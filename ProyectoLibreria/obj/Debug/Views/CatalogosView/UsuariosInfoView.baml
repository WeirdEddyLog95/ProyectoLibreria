   M S B A M L   `   `   `  ����K  GProyectoLibreria, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:ProyectoLibreria.Views.CatalogosView$ProyectoLibreria.Views.CatalogosView  5   	   ;    5ProyectoLibreria.Views.CatalogosView.UsuariosInfoView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Dlocal2clr-namespace:ProyectoLibreria.Views.CatalogosView     5   	     � Title$  UsuariosInfoView��5   	   $	��450��6"   $��822.846��6/   .��5	      �� $��0,0,2,0q�6   .+�5
   
   ]� $��56��6   .+�5      �� -   5        $��titulo1��5      $��Datos del Usuario��6#   $
��Left=�6?   $��10q�6Z   $	��Top=�6f   $��18(�6~   $
��Bold"�6�   ]� 5      $��Right=�6   $��56��64   .+�5      �� -   5        $
��btn1��5      $��40��6%   $��Right=�61   $��70��6M   $��3,8,3,3q�6X   ��5      �� 5       j� Stretch$
 None=�6%    �� ImageSource$= 7/ProyectoLibreria;component/Imagenes/icons8-plus-24.png��64   5      �� 5      -   5        $
��btn2��5      $��40��6%   $��Right=�61   $��70��6M   $��3,8,3,3q�6X   ��5      �� 5      $B </ProyectoLibreria;component/Imagenes/icons8-checkmark-24.png��6%   $
 None=�6p   5      �� 5      -   5        $
��btn3��5      $��40��6%   $��Right=�61   $��70��6M   $��3,8,3,3q�6X   ��5      �� 5      $G A/ProyectoLibreria;component/Imagenes/icons8-pencil-drawing-24.png��6%   $
 None=�6u   5      �� 5      -   5        $
��btn4��5      $��40��6%   $��Right=�61   $��70��6M   $��3,8,3,3q�6X   ��5      �� 5      $? 9/ProyectoLibreria;component/Imagenes/icons8-delete-24.png��6%   $
 None=�6m   5       �� 5!      -   5        $
��btn5��5!      $��40��6%   $��Right=�61   $��70��6M   $��3,8,3,3q�6X   ��5"      �� 5#      $@ :/ProyectoLibreria;component/Imagenes/icons8-refresh-24.png��6%   $
 None=�6n   5%      5&      5'      ]� 5(   
   $��LeftToRight=�6   .+�5)      ��  �� Orientation$ Vertical=�6   .+�5*      �� -   5        $��titulo2��5*      $��Nombre del Usuario:��6'   $��10,0,0,0q�6E   $��38��6W   $��16(�6c   $
��Left=�6q   �� 5+      -   5        $��Nombre_Usuario��5+      $��10,0,0,0q�60   $��30��6B   $��14(�6N   $
��Left=�6\   $	��514��6w   5,      �� 5-      $ Vertical=�6   .+�5.      �� -	   5        $��titulo3��5.      $��Folio de Credencial:��6'   $��10,0,0,0q�6F   $��38��6X   $��16(�6d   $
��Left=�6r   �� 5/      $ 
Horizontal=�6   .+�50      �� -
   5        $��Año_Credencial��50      $��10,0,0,0q�64   $��30��6F   $��14(�6R    �� 	MaxLength$ 4��6`   $
��Left=�6n   $��95��6�   �� 51      -   5        $��Folio_Secuencia��51      $��10,0,0,0q�65   $��30��6G   $��14(�6S   $ 5��6a   $
��Left=�6o   $	��160��6�   52      53      54      ]� 55   
   $��LeftToRight=�6   .+�56      �� $ Vertical=�6   .+�57      �� -   5        $��titulo4��57      $��Fecha de Registro:��6'   $��10,0,0,0q�6D   $��38��6V   $��16(�6b   $
��Left=�6p   �� 58      -   5        $��Fecha_Registro��58      $��10,0,0,0q�6m   $��30��6   $��14(�6�   $
��Left=�6�   $	��170��6�   ��60   ��  �� Path$ DateTimeValue� �� StringFormat$ 
dd/MM/yyyy��6�   59      5:      ]� 5;   
   .+�5<      �� -   5        $��titulo5��5<      $��Usuarios Registrados��6#   $
��Left=�6B   $��	10,10,0,0q�6]   $	��Top=�6p   $��18(�6�   $
��Bold"�6�   5=      ]� 5>   
   $��10q�6   .+�5?      &   System.Windows.Controls.DataGrid  -   5        $��	dataGrid1��5?      $	��Top=�6(      AutoGenerateColumns .     SelectionMode$ Extended=�6\   	   
IsReadOnly	 . 
   Columns
 5@      0  *System.Windows.Controls.DataGridTextColumn  5A      ,  &System.Windows.Controls.DataGridColumn   Header$ Nombre del Usuario��6)   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6E   �� *Nombre_Usuario+  5B      $ Folio del Credencial��6)    6G   �� *Folio_Credencial+  5C      $ Fecha de Registro��6)    6D   �� *Fecha_Registro+5E      5F      5G      5H      