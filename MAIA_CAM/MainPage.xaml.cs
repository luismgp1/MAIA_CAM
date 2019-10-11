using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.Media.Capture;
using Windows.Storage;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.ViewManagement; //full screen

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace MAIA_CAM
{


    public sealed partial class MainPage : Page
    {
        public static int _0interador = 0;
        public static int _1interador = 1;
        public static int cont = 1;
        public static int cantidad = 1;
        public static string nombrefoto = "sample";
        public static string nombredato = "datos.txt";
        public static string inputentrada = "entrada.txt";
        public static string carpetanombre = "camara";
        public static string _1camara = "1camara.jpg";
        public static string _1foto = "1foto.jpg";
        public static string _1nocapturo = "1nocapturo.jpg";

        public MainPage()
        {
            //this.InitializeComponent();


            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
            var view = ApplicationView.GetForCurrentView();
            view.TryEnterFullScreenMode();



            Loaded += async (o, e) =>
            {

                imageControl.Source = await Init();
                // imageControl1.Source = null;
                imageControl.Source = await PhotoTake();

            };

            this.InitializeComponent();
        }

        public async Task<SoftwareBitmapSource> fotosolo()
        {

            var uri1 = new System.Uri("ms-appx:///Assets/" + _1foto);

            var imagen = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri1);


            var bitmapSource = new SoftwareBitmapSource();

            using (var stream = await imagen.OpenAsync(FileAccessMode.Read))
            {
                var decoder = await BitmapDecoder.CreateAsync(stream);
                var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                var softwareBitmapBGR8 = SoftwareBitmap.Convert(
                softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

                await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);
            }

            return bitmapSource;
        }

        public async Task<SoftwareBitmapSource> Init()
        {


            var uri1 = new System.Uri("ms-appx:///Assets/" + _1camara);

            var imagen = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri1);


            var bitmapSource = new SoftwareBitmapSource();

            using (var stream = await imagen.OpenAsync(FileAccessMode.Read))
            {
                var decoder = await BitmapDecoder.CreateAsync(stream);
                var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                var softwareBitmapBGR8 = SoftwareBitmap.Convert(
                softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

                await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);
            }

            return bitmapSource;


        }





        public async Task<SoftwareBitmapSource> PhotoTake()
        {
            Debug.WriteLine("contador entrada tomar foto= " + cont);
            if (cont <= 0)
            {
                cont = 1;
                Debug.WriteLine("si contador <= 0 contador = " + cont);
            }
            cont--;
            Debug.WriteLine("se le resta -1  contador = " + cont);
            var bitmapSource = new SoftwareBitmapSource();

            var uri = new System.Uri("ms-appx:///Assets/" + _1nocapturo);

            var pictureFolder = KnownFolders.PicturesLibrary;
            pictureFolder = await pictureFolder.CreateFolderAsync(carpetanombre, CreationCollisionOption.OpenIfExists);

            var imagenpre = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);//crear variable global
            // var imagenpre = await pictureFolder.GetFileAsync("sample(0).jpg");
            if (cantidad == _0interador)
            {

                imagenpre = await pictureFolder.GetFileAsync(nombrefoto + "(1).jpg"); ///////////////////////IMAGEN 0

            }


            if (cantidad == 1)
            {

                CameraCaptureUI captureUI = new CameraCaptureUI();
                captureUI.PhotoSettings.AllowCropping = false;
                captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
                var photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);////CAPTURAR MAS DE 1 FOTO

                captureUI.PhotoSettings.MaxResolution = Windows.Media.Capture.CameraCaptureUIMaxPhotoResolution.HighestAvailable;

                if (photo == null)
                {
                    cont++;
                    var imagen = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);


                    using (var stream = await imagen.OpenAsync(FileAccessMode.Read))
                    {
                        var decoder = await BitmapDecoder.CreateAsync(stream);
                        var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                        var softwareBitmapBGR8 = SoftwareBitmap.Convert(
                        softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

                        await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);
                    }

                    return bitmapSource;

                }


                if (photo != null)
                {

                    //  imageControl1.Source = null;

                    try
                    {

                        ///VALOR DEL ITERADOR recibido cambiar dependiendo

                        var Entradafile = await pictureFolder.GetFileAsync(inputentrada);
                        string entrada = await FileIO.ReadTextAsync(Entradafile);
                        cantidad = Convert.ToInt32(entrada);



                        // This is where we want to save to.
                        cont++;
                        await photo.CopyAsync(pictureFolder, nombrefoto + "(" + cont + ").jpg", NameCollisionOption.ReplaceExisting);

                        Debug.WriteLine("Photo saved! n= " + cont);
                        imagenpre = photo;

                        // var uri = new System.Uri("ms-appx:///Assets/captura.png");
                        //crear fichero
                        await pictureFolder.CreateFileAsync(nombredato, CreationCollisionOption.ReplaceExisting);
                        var file2 = await pictureFolder.GetFileAsync(nombredato);
                        await FileIO.WriteTextAsync(file2, cont + "\n");


                        using (var stream1 = await photo.OpenAsync(FileAccessMode.Read))
                        {
                            var decoder = await BitmapDecoder.CreateAsync(stream1);
                            var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                            var softwareBitmapBGR8 = SoftwareBitmap.Convert(
                            softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                            await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);
                        }
                        cont++;
                    }
                    catch (Exception ex)
                    {
                        // File I/O errors are reported as exceptions
                        Debug.WriteLine("Exception when taking a photo: " + ex.ToString());
                    }

                }




            }
            else
            {






                using (var stream1 = await imagenpre.OpenAsync(FileAccessMode.Read))
                {
                    var decoder = await BitmapDecoder.CreateAsync(stream1);
                    var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                    var softwareBitmapBGR8 = SoftwareBitmap.Convert(
                    softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                    await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);
                }
                // imageControl1.Source = await fotosolo(); <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<FOTO SOLO

                return bitmapSource;
            }

            return bitmapSource;

        }



        public async void DeletePhoto()
        {
            Debug.WriteLine("contador entrada delete foto = " + cont);
            try
            {

                var bitmapSource = new SoftwareBitmapSource();
                var uri = new Uri("ms-appx:///Assets/" + _1camara);

                var picturefolder = KnownFolders.PicturesLibrary;
                picturefolder = await picturefolder.CreateFolderAsync(carpetanombre, CreationCollisionOption.OpenIfExists);





                if (cantidad == _1interador)
                {
                    Debug.WriteLine("Borrar Foto Cantidad=" + cantidad);



                    if (cont <= 0 && cont != 1)
                    {
                        Debug.WriteLine("contador (cont <= 0 && cont !=1) valor de contador = " + cont);

                        var nofoto = await StorageFile.GetFileFromApplicationUriAsync(uri);
                        using (var stream1 = await nofoto.OpenAsync(FileAccessMode.Read))
                        {
                            var decoder = await BitmapDecoder.CreateAsync(stream1);
                            var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                            var softwareBitmapBGR8 = SoftwareBitmap.Convert(
                            softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                            await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);
                        }

                        imageControl.Source = bitmapSource;
                        cont = 1;

                        Debug.WriteLine("No hay tomada contador =" + cont);
                    }


                    // Mostrar Foto
                    if (cont >= 1)
                    {
                        Debug.WriteLine("borra foto valor entrando  =" + cont);
                        cont--;

                        Debug.WriteLine("volar de contador al borrar -1 = " + cont);

                        var fotoborrar = await picturefolder.GetFileAsync(nombrefoto + "(" + cont + ").jpg");
                        await fotoborrar.DeleteAsync();
                        Debug.WriteLine("volar de contador al borrar -1 numero de la foto borrada = " + cont);


                        await picturefolder.CreateFileAsync(nombredato, CreationCollisionOption.ReplaceExisting);
                        var Samplefile = await picturefolder.GetFileAsync(nombredato);



                        if (cont == 1)
                        {
                            var nofoto = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);
                            using (var stream1 = await nofoto.OpenAsync(FileAccessMode.Read))
                            {
                                var decoder = await BitmapDecoder.CreateAsync(stream1);
                                var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                                var softwareBitmapBGR8 = SoftwareBitmap.Convert(
                                softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                                await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);
                            }

                            imageControl.Source = bitmapSource;
                            cont = 1;
                            await FileIO.WriteTextAsync(Samplefile, cont + "\n");
                            Debug.WriteLine("Foto ultima foto contador = " + cont);
                        }
                        else
                        {
                            int cont1 = cont - 1;
                            var nofoto = await picturefolder.GetFileAsync(nombrefoto + "(" + cont1 + ").jpg");
                            using (var stream1 = await nofoto.OpenAsync(FileAccessMode.Read))
                            {
                                var decoder = await BitmapDecoder.CreateAsync(stream1);
                                var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                                var softwareBitmapBGR8 = SoftwareBitmap.Convert(
                                softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                                await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);
                            }

                            imageControl.Source = bitmapSource;
                            await FileIO.WriteTextAsync(Samplefile, cont1 + "\n");
                            Debug.WriteLine("Foto mostrada n= " + cont1);
                        }
                    }
                }


                if (cantidad == _0interador)
                {
                    Debug.WriteLine("Borrar Foto Cantidad=" + cantidad);

                    if (cont == 2)
                    {
                        cont = 1;
                        Debug.WriteLine("contador =2 cambiamos a contador =1");
                    }

                    if (cont == 1)
                    {
                        Debug.WriteLine("si contador == 1" + cont);
                        var file = await picturefolder.GetFileAsync(nombrefoto + "(" + cont + ").jpg");
                        cont--;
                        Debug.WriteLine("si contador == 1, le restamos 1 =" + cont);
                        await file.DeleteAsync();
                        await picturefolder.CreateFileAsync(nombredato, CreationCollisionOption.ReplaceExisting);
                        var Samplefile = await picturefolder.GetFileAsync(nombredato);
                        await FileIO.WriteTextAsync(Samplefile, cont + "\n");
                        cantidad = 1;
                        Debug.WriteLine("colocamos cantidad =" + cantidad);
                    }

                    if (cont <= 0)
                    {
                        Debug.WriteLine("si contador <=0 , contador =" + cont);
                        cont = 1;
                        Debug.WriteLine("se cambia el valor a 1 contador =" + cont);
                        var nofoto = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);
                        using (var stream1 = await nofoto.OpenAsync(FileAccessMode.Read))
                        {
                            var decoder = await BitmapDecoder.CreateAsync(stream1);
                            var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                            var softwareBitmapBGR8 = SoftwareBitmap.Convert(
                            softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                            await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);
                        }

                        imageControl.Source = bitmapSource;

                        var Samplefile = await picturefolder.GetFileAsync(nombredato);
                        await FileIO.WriteTextAsync(Samplefile, cont + "\n");
                        cantidad = 1;
                        Debug.WriteLine("colocamos cantidad =" + cantidad);

                    }


                }

            }
            catch (Exception ex)
            {

                // File I/O errors are reported as exceptions
                Debug.WriteLine("Exception when Botton Borrar recording: " + ex.ToString());
            }
        }


        public async void Cerrabutton()
        {
            var picturefolder = KnownFolders.PicturesLibrary;
            picturefolder = await picturefolder.CreateFolderAsync(carpetanombre, CreationCollisionOption.OpenIfExists);
            var EditEntrada = await picturefolder.GetFileAsync(inputentrada);

            // await FileIO.WriteTextAsync(EditEntrada, "3");
            // string cerrarEntrada = await FileIO.ReadTextAsync(EditEntrada);

            // int control = Convert.ToInt32(cerrarEntrada);
            int control = 3;

            if (control == 3)
            {
                // EditEntrada = await picturefolder.GetFileAsync("datos.txt");
                // await EditEntrada.DeleteAsync();

                Application.Current.Exit();
            }


        }


        public async void Buttonfoto(object sender, RoutedEventArgs e)
        {
            imageControl.Source = await PhotoTake();
        }


        public async void Borrar_Click(object sender, RoutedEventArgs e)
        {
            DeletePhoto();
        }

        public async void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            Cerrabutton();
        }
    }





}
