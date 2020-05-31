using System;

public class ContextMenuItem
{
    public string Text { get; set; }
    public Action Click { get; set; }
    public int ActionId { get; set; }
}