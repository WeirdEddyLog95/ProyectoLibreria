   M S B A M L   `   `   `  ����K  GProyectoLibreria, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:ProyectoLibreria.Views.CatalogosView$ProyectoLibreria.Views.CatalogosView  5   	   9    3ProyectoLibreria.Views.CatalogosView.LibrosInfoView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Dlocal2clr-namespace:ProyectoLibreria.Views.CatalogosView     5   	     � Title$  Catalogo de Libros��5   	   $��557.308��6$   $��1124.423��65   .��5	      �� $��0,0,2,0q�6   .+�5
   
   ]� $��56��6   .+�5      �� -   5        $��titulo1��5      $��Datos del Libro��6#   $
��Left=�6=   $��10q�6X   $	��Top=�6d    J� RenderTransformOrigin$ -1.754,-1.614&�6|   $��18(�6�   $
��Bold"�6�   ]� 5      $��Right=�6   $��56��64   .+�5      �� -   5        $
��btn1��5      $��40��6%   $��Right=�61   $��70��6M   $��3,8,3,3q�6X   ��5      �� 5       j� Stretch$
 None=�6%    �� ImageSource$= 7/ProyectoLibreria;component/Imagenes/icons8-plus-24.png��64   5      �� 5      -   5        $
��btn2��5      $��40��6%   $��Right=�61   $��70��6M   $��3,8,3,3q�6X   ��5      �� 5      $B </ProyectoLibreria;component/Imagenes/icons8-checkmark-24.png��6%   $
 None=�6p   5      �� 5      -   5        $
��btn3��5      $��40��6%   $��Right=�61   $��70��6M   $��3,8,3,3q�6X   ��5      �� 5      $G A/ProyectoLibreria;component/Imagenes/icons8-pencil-drawing-24.png��6%   $
 None=�6u   5      �� 5      -   5        $
��btn4��5      $��40��6%   $��Right=�61   $��70��6M   $��3,8,3,3q�6X   ��5      �� 5      $? 9/ProyectoLibreria;component/Imagenes/icons8-delete-24.png��6%   $
 None=�6m   5       �� 5!      -   5        $
��btn5��5!      $��40��6%   $��Right=�61   $��70��6M   $��3,8,3,3q�6X   ��5"      �� 5#      $@ :/ProyectoLibreria;component/Imagenes/icons8-refresh-24.png��6%   $
 None=�6n   5%      5&      5'      ]� 5(   
   $��LeftToRight=�6   .+�5)      ��  �� Orientation$ Vertical=�6   .+�5*      �� -   5        $��titulo2��5*      $��Titulo del Libro:��6'   $��10,0,0,0q�6C   $��38��6U   $��16(�6a   $
��Left=�6o   �� 5+      -   5        $��Titulo_Libro��5+      $��10,0,0,0q�6.   $��30��6@   $��14(�6L   $
��Left=�6Z   $	��450��6u   5,      �� 5-      $ Vertical=�6   .+�5.      �� -	   5        $��titulo3��5.      $��Autor del Libro:��6'   $��10,0,0,0q�6B   $��38��6T   $��16(�6`   $
��Left=�6n   �� 5/      -
   5        $��Autor_Libro��5/      $��10,0,0,0q�6-   $��30��6?   $��14(�6K   $
��Left=�6Y   $	��450��6t   50      �� 51      $ Vertical=�6   .+�52      �� -   5        $��titulo4��52      $��Fecha de Publicacion:��6'   $��10,0,0,0q�6G   $��38��6Y   $��16(�6e   $
��Left=�6s   �� 53      -   5        $��Fecha_Publicacion��53      $��10,0,0,0q�6p   $��30��6�   $��14(�6�   $
��Left=�6�   $	��170��6�   ��63   ��  �� Path$ DateTimeValue� �� StringFormat$ 
dd/MM/yyyy��6�   54      55      ]� 56   
   $��56��6   .+�57      �� -   5        $��titulo7a��57      $��Detalles del Libro��6$   $
��Left=�6A   $��10q�6\   $	��Top=�6h   $��18(�6�   $
��Bold"�6�   58      ]� 59   
   $��LeftToRight=�6   .+�5:      �� $ Vertical=�6   .+�5;      �� -   5        $��titulo5��5;      $��Clave del Genero:��6'   $��10,0,0,0q�6C   $��38��6U   $��16(�6a   $
��Left=�6o   �� 5<      -   5        $��Clave_Genero��5<      $��10,0,0,0q�6/   $	��150��6A   $��40��6M   $��16(�6Y   $��Center=�6g   $	��Top=�6�   $��Visible=�6�   ��.  6�   5=      �� 5>      $ Vertical=�6   .+�5?      �� -   5        $��titulo6��5?      $��Clave del Pasillo:��6'   $��10,0,0,0q�6D   $��38��6V   $��16(�6b   $
��Left=�6p   �� 5@      -   5        $��Clave_Pasillo��5@      $��10,0,0,0q�60   $	��150��6B   $��40��6N   $��16(�6Z   $��Center=�6h   $	��Top=�6�   $��Visible=�6�   ��.  6�   5A      �� 5B      $ Vertical=�6   .+�5C      �� -   5        $��titulo7��5C      $��Clave del Libro:��6'   $��10,0,0,0q�6B   $��38��6T   $��16(�6`   $
��Left=�6n   �� 5D      $ 
Horizontal=�6   .+�5E      �� -   5        $��Clave_Libro��5E      $��10,0,0,0q�61   $��30��6C   $��14(�6O   $
��Left=�6]   $	��150��6x   5F      5G      �� 5H      $ Vertical=�6   .+�5I      �� -   5        $��titul08��5I      $��Estado:��6'   $��10,0,0,0q�69   $��38��6K   $��14(�6W   $
��Left=�6e   �� 5J      -   5        $��Estado��5J      $��10,0,0,0q�6A   $	��150��6S   $��40��6_   $��16(�6k   $
��Left=�6y   $	��Top=�6�   $��Visible=�6�   ��.  ��6)   �� .@�5K      �� .��6$   
Prestamo6.   �� 5L      .��6#   
Disponible6/   �� 5M      .��6#   Baja6)   5N      5P      5Q      ]� 5R   
   $��56��6   .+�5S      �� -   5        $��titulo7b��5S      $��Libros Registrados��6$   $
��Left=�6A   $��10q�6\   $	��Top=�6h   $��18(�6�   $
��Bold"�6�   5T      ]� 5U   
   $��10q�6   .+�5V      &   System.Windows.Controls.DataGrid  -   5        $��	dataGrid1��5V      $	��Top=�6(      AutoGenerateColumns .     SelectionMode$ Extended=�6\   	   
IsReadOnly	 . 
   Columns
 5W      0  *System.Windows.Controls.DataGridTextColumn  5X      ,  &System.Windows.Controls.DataGridColumn   Header$ Titulo del Libro��6)   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6C   �� *Titulo_Libro+  5Y      $ Autor del Libro��6)    6B   �� *Autor_Libro+  5Z      $ Fecha de Publicacion��6)    6G   �� *Fecha_Publicacion+  5[      $ Clave del Genero��6)    6C   �� *Clave_Genero+  5\      $ Clave del Pasillo��6)    6D   �� *Clave_Pasillo+  5]      $ Clave del Libro��6)    6B   �� *Clave_Libro+  5^      $ Estado del Libro��6)    6C   �� *Estado+5`      5a      5b      5c      