   M S B A M L   `   `   `  ����K  GProyectoLibreria, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:ProyectoLibreria.Views.PrestamosView$ProyectoLibreria.Views.PrestamosView  5   	   8    2ProyectoLibreria.Views.PrestamosView.PrestamosView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Dlocal2clr-namespace:ProyectoLibreria.Views.PrestamosView     5   	     � Title$  Generacion de Prestamos��5   	   $��412.34��6)   $��711.898��69   .��5	      �� .+�5
   
   ]� .+�5      �� -   5        $��titulo1��5      $��WrapWithOverflow=�6'   $	��350��6G   $
��Left=�6S   $��10q�6n   $	��Top=�6z   $��16(�6�   $
��Bold"�6�   {�.  ?=Favor de anotar los campos para generar un prestamo de libros5      ]� 5      $��Right=�6   $��56��64   .+�5      �� -   5        $
��btn1��5      $��40��6%   $��Right=�61   $	��120��6M   $��3,8,3,3q�6Y   ��5      �� 5       j� Stretch$
 None=�6%    �� ImageSource$B </ProyectoLibreria;component/Imagenes/icons8-checkmark-24.png��64   5      �� 5      -   5        $
��btn2��5      $��40��6%   $��Right=�61   $	��120��6M   $��3,8,3,3q�6Y   ��5      �� 5      $? 9/ProyectoLibreria;component/Imagenes/icons8-delete-24.png��6%   $
 None=�6m   5      5      5      ]� 5   
   $��LeftToRight=�6   .+�5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $ Vertical=�6   .+�5      �� $��Folio de Credencial:��6   $��10,0,0,0q�6;   $��38��6M   $��16(�6Y   $
��Left=�6g   �� 5      -   5        $��Folio_Credencial��5       �� 	MaxLength$ 10��66   $��10,0,0,0q�6E   $��30��6W   $��14(�6c   $
��Left=�6q   $	��160��6�   6�   5       �� 5!      $ Vertical=�6   .+�5"      �� $��Nombre del Usuario:��6   $��10,0,0,0q�6:   $��38��6L   $��16(�6X   $
��Left=�6f   �� 5#      -   5        $��Nombre_Usuario��5#      $��10,0,0,0q�64   $��30��6F   $��14(�6R   $
��Left=�6`   $	��400��6{    �� 
IsReadOnly . 6�   5$      5%      5&      ]� 5'   
   $��LeftToRight=�6   .+�5(      �� $ 
Horizontal=�6   .+�5)      �� $ Vertical=�6   .+�5*      �� $��Clave del Libro:��6   $��10,0,0,0q�67   $��38��6I   $��16(�6U   $
��Left=�6c   �� 5+      -   5        $��Clave_Libro��5+      $ 7��61   $��10,0,0,0q�6?   $��30��6Q   $��14(�6]   $
��Left=�6k   $	��160��6�    . 6�   5,      5-      5.      ]� 5/   
   $��10q�6   .+�50      &   System.Windows.Controls.DataGrid  -   5        $��	dataGrid1��50      $	��Top=�6(      AutoGenerateColumns .     SelectionMode$ Extended=�6\      
IsReadOnly . 	   Columns	 51      0  *System.Windows.Controls.DataGridTextColumn  52      ,  &System.Windows.Controls.DataGridColumn
   Header$
 Clave del Libro��6)   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6B   �� *Clave_Libro+  53      $
 Titulo del Libro��6)    6C   �� *Titulo_Libro+  54      $
 Autor del Libro��6)    6B   �� *Autor_Libro+  55      $
 Estado del Libro��6)    6C   �� *Estado+57      58      59      5:      