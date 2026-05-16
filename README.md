# Compilador MicroC

## 📌 Portada 

**Nombre completo:** Angel Hernández\
**Número de carné:** 202425514\
**Curso:** Autómatas y Lenguajes\
**Proyecto:** Compilador MicroC

------------------------------------------------------------------------

## 📖 Descripción del Proyecto

MicroC es un pre-compilador desarrollado como proyecto del curso de
Autómatas y Lenguajes de la Universidad Mesoamericana.

El objetivo principal del proyecto es simular el funcionamiento básico
de un compilador para el lenguaje C, permitiendo al usuario escribir
código, guardarlo, abrir archivos existentes y verificar errores
sintácticos simples.

El sistema analiza el código ingresado y valida:

-   Existencia de la función principal `int main()`
-   Correcto uso de llaves `{ }`
-   Presencia de punto y coma `;`
-   Estructura básica del programa

También incluye control de edición, confirmación de salida con cambios
sin guardar y ventana de ayuda integrada.

------------------------------------------------------------------------

## 🛠 Tecnologías Utilizadas

-   Lenguaje: C#
-   Framework: .NET 8
-   Interfaz gráfica: Avalonia UI
-   Control de versiones: Git y GitHub
-   Sistema operativo de desarrollo: Linux

------------------------------------------------------------------------

## ▶ Instrucciones de Ejecución

### 🔹 Ejecutar desde el código fuente

1.  Abrir una terminal dentro de la carpeta del proyecto.
2.  Ejecutar el siguiente comando:

```{=html}
<!-- -->
```
    dotnet run

------------------------------------------------------------------------

### 🔹 Ejecutar versión compilada (Release)

1.  Navegar a la carpeta:

```{=html}
<!-- -->
```
    bin/Release/net8.0/linux-x64/publish/

2.  Ejecutar:

```{=html}
<!-- -->
```
    ./MicroC

También puede ejecutarse desde el acceso directo creado en el escritorio
(si aplica).

------------------------------------------------------------------------

## 🖼 Capturas de Pantalla


-   Interfaz principal del programa

![Interfaz Principal](assets/interfaz.png)

-   Ejemplo de compilación exitosa

![Interfaz Principal](assets/compilacion.png)

-   Ejemplo de error detectado

![Interfaz Principal](assets/error1.png)

![Interfaz Principal](assets/error2.png)

-   Ventana de ayuda

![Interfaz Principal](assets/ayuda.png)


Las imágenes estan almacenadas en la carpeta `/assets/` según la estructura solicitada.

------------------------------------------------------------------------

## 🎥 Enlace al Video Demostrativo

El video demostrativo muestra:

-   Creación de archivo nuevo
-   Edición de código
-   Guardado de archivo
-   Proceso de compilación
-   Detección de errores
-   Funcionamiento del botón Ayuda

🔗 Enlace al video: https://youtu.be/8Fx_sU_dBjY

------------------------------------------------------------------------

## 📂 Estructura del Repositorio

El repositorio está organizado de la siguiente manera:

    /src/        → Código fuente de la aplicación
    /assets/     → Recursos como imágenes o íconos
    /docs/       → Documentación y capturas de pantalla
    /test/       → (Opcional) Archivos de prueba
    README.md    → Documentación principal del proyecto

------------------------------------------------------------------------

## 📄 Documentación

La documentación completa del proyecto, incluyendo el manual de usuario, descripción técnica y capturas de pantalla, se encuentra disponible en la carpeta /docs/ dentro de este repositorio.

En dicha carpeta se incluyen:

Manual de Usuario en formato Word (.docx)

Capturas de pantalla del sistema

Documentación complementaria del proyecto

## 🚀 Versión Final

Se publicó un Release final en GitHub con el tag:

v1.0-precompilador

Esta versión contiene el código funcional completo del pre-compilador
MicroC.