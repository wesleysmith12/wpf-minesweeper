   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	     � Title$  Minesweeper��5   	   $
��1000��6   $
��1000��6+   .��5
      �� .+�5   
   ]� .+�5      o� $	��Top=�6   .@�5      m� $��_File��6   .@�5      m� -   5        $��_Easy��5      6.   m� 5      -   5        $��_Normal��5      60   m� 5      -   5        $��_Hard��5      6.   5      5      �� 5      6   5      �� 5   
    �� Orientation$ Vertical=�6   $��Center=�6,   $��	0,20,0,10q�6I   .+�5      � $��0,0,0,5q�6    � ColumnDefinitions 5      �� 5      6)   �� 5      6)   �� 5      6)    � RowDefinitions 5      �� 5      6&   .+�5!      �� -   5        $��gameTime��5!      
���   �$��0��6?   $��0��6H   $��16(�6X   $��Center=�6f   $
��Left=�6�   ���	#FFF60606$
��Bold"�6�   $��5,0,0,0q�6�   $��Arial)�6�   �� 5"      -   5        $��0��5"      $��1��6&   $��16(�66   $��Center=�6D   $��Center=�6_   $
��10,0q�6|   $
��Bold"�6�   .��6�   Reset6�   �� 5#      -   5        $��	bombCount��5#      
���   �$��5��6@   $��2��6I   $��16(�6Y   $��Center=�6g   $��Right=�6�   ���	#FFF60606$
��Bold"�6�   $��0,0,5,0q�6�   5$      �� 5%      -   5        $��myArea��5%      5'      ]� 5(      .+�5+      �� -   5        $��
hintButton��5+      $��Need a hint?��6+   6B   5-      �� 5.      High Scores6%   �� 5/      -	   5        $
��easy��5/      	Easy: -6-   �� 50      -
   5        $��normal��50      	Normal: -61   �� 51      -   5        $
��hard��51      	Hard: -6-   5;      5>      5B      