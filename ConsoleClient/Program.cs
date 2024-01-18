using ConsoleClient;

ImageUploader imageUploader = new ImageUploader().Port(8000).Server("localhost").Connect();

imageUploader.Upload("/Users/davidnam/Documents/BCIT/COMP 4945/Assignments/comp4945assignment1/ConsoleClient/Images/Goat.jpeg");
imageUploader.Close();