   M S B A M L   `   `   `  ����K  GProyectoLibreria, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:ProyectoLibreria.Views.CatalogosView$ProyectoLibreria.Views.CatalogosView  5   	   9    3ProyectoLibreria.Views.CatalogosView.GeneroInfoView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Dlocal2clr-namespace:ProyectoLibreria.Views.CatalogosView     5   	     � Title$  Catalogo de Generos��5   	   $	��576��6%   .��5	      �� $��0,0,2,7q�6   .+�5
   
   ]� $��56��6   .+�5      �� -   5        $��titulo1��5      $��Datos del Genero��6#   $
��Left=�6>   $��10q�6Y   $	��Top=�6e    J� RenderTransformOrigin$ -1.754,-1.614&�6}   $��18(�6�   $
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
 None=�6n   5%      5&      5'      �� 5(   
   -   5        $��titulo2��5(      $��Nombre del Genero:��6   $��10,0,0,0q�6<   $��38��6N   $��16(�6Z   $
��Left=�6h   �� 5)   
   -   5        $��Nombre_Genero��5)      $��10,0,0,0q�6'   $��30��69   $��14(�6E   $
��Left=�6S   $	��514��6n   �� 5*   
   -	   5        $��titulo3��5*      $��Clave del Genero:��6   $��10,0,0,0q�6;   $��38��6M   $��16(�6Y   $
��Left=�6g   �� 5+   
   -
   5        $��Clave_Genero��5+      $��10,0,0,0q�6&   $��30��68   $��14(�6D   $
��Left=�6R   $	��320��6m   ]� 5,   
   $��10q�6   .+�5-      &   System.Windows.Controls.DataGrid  -   5        $��	dataGrid1��5-      $	��Top=�6(      AutoGenerateColumns .     SelectionMode$ Extended=�6\      
IsReadOnly .    Columns 5.      0  *System.Windows.Controls.DataGridTextColumn  5/      ,  &System.Windows.Controls.DataGridColumn   Header$ Nombre del Genero��6)   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 6D   �� *Nombre_Genero+6h     50      $ Clave del Genero��6)   	 6C   �� *Clave_Genero+6f   52      53      54      55      