﻿using Microsoft.UI.Xaml.Controls;


namespace ProjetoAcessibilidade.Views;

/// <summary>
/// Page content to send to the printer
/// </summary>
public sealed partial class PageToPrint : Page
{
    public RichTextBlock TextContentBlock
    {
        get; set;
    }

    public PageToPrint()
    {
        this.InitializeComponent();
        TextContentBlock = TextContent;
    }
}
