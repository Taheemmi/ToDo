Hello people, so i made this inside the advanced folder to show the progress on how to make seperet class files, currently everything works but all the tasks are saved are seen by all users. Any questions feel free to ask.

so we have added a feature to allow for users to save their own tasks. however they have to press enter again after they typed their password.

Here officer, this was the issue:

if (authenticated)
{
  currentUser = Console.ReadLine();
}
