var (left, top) = Console.GetCursorPosition();
Console.CursorVisible = false;

while (true)
{
    Console.SetCursorPosition(left, top);
    Console.WriteLine(new Random().Next(0, 100));
    Thread.Sleep(100);
}