using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que representa una ventana donde se alojan otras ventanas
/// Y las muestra como una opción en los botones suaves 
/// </summary>
public class DisplayMenu : DisplayGUI
{
    /// <summary>
    /// Ventanas que se alojan en este menú
    /// </summary>
    public DisplayGUI[] Ventanas;
    DisplayGUI[] VentanasActivas = new DisplayGUI[5];
    int PaginaActual = 0;

    /// <summary>
    /// Muestra el menu
    /// </summary>
    public override void Accionar()
    {
        if (DisplayManager.Pantalla.MenuActivo != null && transform.parent != DisplayManager.Pantalla.MenuActivo.transform)
            DisplayManager.Pantalla.MenuActivo.gameObject.SetActive(false);
        gameObject.SetActive(true);
        DisplayManager.Pantalla.MenuActivo = this;
        ActualizarOpciones();
        if (VentanasActivas[0] is DisplayMenu menu)
        {
            if (menu.Ventanas[0] is DisplayVentana)
                menu.Preview();
        }
        else if (VentanasActivas[0] is DisplayVentana)
        {
            Opcion(0);
        }
    }

    public void Opcion(int opcion)
    {
        if (opcion < VentanasActivas.Length && VentanasActivas[opcion] != null)
        {
            DisplayManager.Pantalla.OPRT = null;
            ActualizarOpciones();
            VentanasActivas[opcion].Accionar();
            if (VentanasActivas[opcion] is DisplayVentana ventana)
            {
                DisplayManager.Pantalla.OpcionesDisplay[opcion].Resltar();
                if (ventana.OPRT != null)
                {
                    DisplayManager.Pantalla.OPRT = ventana.OPRT;
                    DisplayManager.Pantalla.OpcionesDisplay[4].Texto("OPRT");
                }
            }
        }
    }

    void Preview()
    {
        Ventanas[0].Accionar();
        gameObject.SetActive(true);
        DisplayManager.Pantalla.TituloMenu.text = Titulo;
    }

    public void SigPagina()
    {
        if (PaginaActual < Ventanas.Length / 5)
        {
            PaginaActual++;
            ActualizarOpciones();
        }
    }
    public void AntPagina()
    {
        if (PaginaActual > 0)
        {
            PaginaActual--;
            ActualizarOpciones();
        }
        else if (transform.parent.TryGetComponent(out DisplayMenu menu))
        {
            menu.Accionar();
        }  
    }

    void LimpiarOpciones()
    {
        for (int i = 0; i < DisplayManager.Pantalla.OpcionesDisplay.Length; i++)
        {
            DisplayManager.Pantalla.OpcionesDisplay[i].DesResltar();
            DisplayManager.Pantalla.OpcionesDisplay[i].Texto("  ");
        }
    }

    void SelVentanasActivas()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i + PaginaActual * 5 >= Ventanas.Length)
                VentanasActivas[i] = null;
            else
                VentanasActivas[i] = Ventanas[i + PaginaActual * 5];
        }
    }

    void ActualizarOpciones()
    {
        LimpiarOpciones();
        SelVentanasActivas();
        for (int i = 0; i < 5; i++)
        {
            if (VentanasActivas[i] != null)
            {
                DisplayManager.Pantalla.OpcionesDisplay[i].Texto(VentanasActivas[i].TituloOpcion);
            }
        }
    }
}