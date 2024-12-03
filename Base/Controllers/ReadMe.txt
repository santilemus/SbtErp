Folder Description

The "Controllers" project folder is intended for storing platform-agnostic Controller classes 
that can change the default XAF application flow and add new features.


Relevant Documentation

Extend Functionality
http://help.devexpress.com/#Xaf/CustomDocument2623

Controller Class
http://help.devexpress.com/#Xaf/clsDevExpressExpressAppControllertopic

ViewController Class
http://help.devexpress.com/#Xaf/clsDevExpressExpressAppViewControllertopic

WindowController Class
http://help.devexpress.com/#Xaf/clsDevExpressExpressAppWindowControllertopic

Notas:
27/oct/2024
Se excluye ValidationControllerBase porque genera excepción (no siempre) por operación de hilo cruzado, 
cuando se edita el detalle. Se debe buscar otra manera de implementar la función para comprobar que los 
datos el encabezado son correctos antes de ingresar detalles, que es la finalidad del controller retirado
o en su defecto limitarlo para ejecutar solo las validaciones RequiredField.
