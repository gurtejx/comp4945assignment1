using ConsoleClient;

ImageUploader imageUploader = new ImageUploader().Port(8000).Server("127.0.0.1").Connect();

imageUploader.Upload("../../../Images/Goat.jpeg");
imageUploader.Close();
