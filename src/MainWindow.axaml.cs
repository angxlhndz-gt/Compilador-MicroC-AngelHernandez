using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Layout;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MicroC;

public partial class MainWindow : Window
{
    private const string ArchivoAyuda = "COMPILADOR2026.pdf";

    private string? archivoActual = null;
    private bool hayCambios = false;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void EditorTextBox_TextChanged(object? sender, RoutedEventArgs e)
    {
        hayCambios = true;
    }

    private void Nuevo_Click(object? sender, RoutedEventArgs e)
    {
        EditorTextBox.Text = "";
        OutputTextBox.Text = "";
        archivoActual = null;
        hayCambios = false;
        this.Title = "MicroC - Sin archivo";
        EditorTextBox.IsReadOnly = false;
    }

    private async void Abrir_Click(object? sender, RoutedEventArgs e)
    {
        var archivos = await StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                Title = "Abrir archivo MicroC",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Archivo C")
                    {
                        Patterns = new[] { "*.c" }
                    }
                }
            });

        if (archivos.Count > 0)
        {
            var archivo = archivos[0];
            archivoActual = archivo.Path.LocalPath;

            var contenido = await File.ReadAllTextAsync(archivoActual);
            EditorTextBox.Text = contenido;

            EditorTextBox.IsReadOnly = true;
            hayCambios = false;

            this.Title = $"MicroC - {Path.GetFileName(archivoActual)}";
            OutputTextBox.Text = "Archivo cargado correctamente.";
        }
    }

    private void Editar_Click(object? sender, RoutedEventArgs e)
    {
        if (archivoActual != null)
        {
            EditorTextBox.IsReadOnly = false;
            OutputTextBox.Text = "Modo edición activado.";
        }
        else
        {
            OutputTextBox.Text = "No hay archivo abierto.";
        }
    }

    private async void Guardar_Click(object? sender, RoutedEventArgs e)
    {
        if (archivoActual == null)
        {
            var archivo = await StorageProvider.SaveFilePickerAsync(
                new FilePickerSaveOptions
                {
                    Title = "Guardar archivo MicroC",
                    DefaultExtension = "c",
                    FileTypeChoices = new[]
                    {
                        new FilePickerFileType("Archivo C")
                        {
                            Patterns = new[] { "*.c" }
                        }
                    }
                });

            if (archivo != null)
            {
                archivoActual = archivo.Path.LocalPath;
                await File.WriteAllTextAsync(archivoActual, EditorTextBox.Text);
                hayCambios = false;
                this.Title = $"MicroC - {Path.GetFileName(archivoActual)}";
                OutputTextBox.Text = "Archivo guardado correctamente.";
            }
        }
        else
        {
            await File.WriteAllTextAsync(archivoActual, EditorTextBox.Text);
            hayCambios = false;
            OutputTextBox.Text = "Archivo sobrescrito correctamente.";
        }
    }

    private async void Salir_Click(object? sender, RoutedEventArgs e)
    {
        if (hayCambios)
        {
            var confirmacion = await MostrarConfirmacion("Hay cambios sin guardar. ¿Deseas salir?");
            if (!confirmacion)
                return;
        }

        Close();
    }

    private async Task<bool> MostrarConfirmacion(string mensaje)
    {
        var dialog = new Window
        {
            Width = 350,
            Height = 150,
            Title = "Confirmación"
        };

        var panel = new StackPanel
        {
            Margin = new Avalonia.Thickness(15)
        };

        panel.Children.Add(new TextBlock { Text = mensaje });

        var boton = new Button
        {
            Content = "Aceptar",
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Avalonia.Thickness(10)
        };

        boton.Click += (_, __) => dialog.Close(true);

        panel.Children.Add(boton);

        dialog.Content = panel;

        var resultado = await dialog.ShowDialog<bool>(this);
        return resultado;
    }

    private void Compilar_Click(object? sender, RoutedEventArgs e)
    {
        string codigo = EditorTextBox.Text ?? "";

        if (string.IsNullOrWhiteSpace(codigo))
        {
            OutputTextBox.Text = "Error: No hay código para analizar.";
            return;
        }

        // Ejecutar analizador lexico.
        var analizador = new AnalizadorLexico();
        var tokens = analizador.AnalisisLexico(codigo);
        var salida = new StringBuilder();
        int erroresLexicos = 0;

        // Mostrar tokens y contar errores lexicos.
        foreach (var token in tokens)
        {
            salida.AppendLine(token.ToString());

            if (token.Tipo == "ErrorLexico")
            {
                erroresLexicos++;
            }
        }

        salida.AppendLine();
        salida.AppendLine($"Total de tokens: {tokens.Count}");
        salida.AppendLine($"Errores léxicos: {erroresLexicos}");

        OutputTextBox.Text = salida.ToString();
    }

    private void Ayuda_Click(object? sender, RoutedEventArgs e)
    {
        var rutaPdf = ObtenerRutaPdfAyuda();

        if (rutaPdf == null)
        {
            OutputTextBox.Text = "No se encontro el PDF de ayuda en la carpeta docs.";
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = rutaPdf,
                UseShellExecute = true
            });

            OutputTextBox.Text = $"Abriendo ayuda: {Path.GetFileName(rutaPdf)}";
        }
        catch (Exception ex)
        {
            OutputTextBox.Text = $"No se pudo abrir el PDF de ayuda: {ex.Message}";
        }
    }

    private static string? ObtenerRutaPdfAyuda()
    {
        string[] rutas =
        {
            Path.Combine(Directory.GetCurrentDirectory(), "docs", ArchivoAyuda),
            Path.Combine(Directory.GetCurrentDirectory(), "..", "docs", ArchivoAyuda),
            Path.Combine(AppContext.BaseDirectory, "docs", ArchivoAyuda),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "docs", ArchivoAyuda)
        };

        foreach (var ruta in rutas)
        {
            var rutaCompleta = Path.GetFullPath(ruta);

            if (File.Exists(rutaCompleta))
            {
                return rutaCompleta;
            }
        }

        return null;
    }
}
