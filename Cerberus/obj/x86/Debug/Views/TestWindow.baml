   M S B A M L   `   `   `  ����  Cerberus    Cerberus.TestWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  
TestWindow��5   	   $	��400��6   $	��600��6)   .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��80 �6    � ColumnDefinitions 5
   
   �� 5      $	��.6* �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0��6!   $��30��61   $	��160��6@   $��	10,10,0,0q�6O   $
��Left=�5      $	��Top=�6/   $
��Wrap=�6G   ��5      �� *WelcomeText+ �� UpdateSourceTrigger$ PropertyChanged=��� 5   
   $��1��6   $��	Add Items��6!   $��30��65   $	��120��6A   $
��Left=�5      $	��Top=�6-   $��10q�6E   ��5      �� *
RunCommand+�� 5   
   -   5        $��actualLaborMinutesTextBox��5      $��25��63   $	��180��6?   $
��Left=�5      $��Center=�6/   $��
10,0,0,120q�6J   ��5      �� *ActualLaborMinutes+ �� Mode$ TwoWay=�$ PropertyChanged=� �� ValidatesOnDataErrors . 5   J   �� 5   
   $��1��6   $	��250��6"   $	��220��6/   $��Right=�6;   $	��Top=�6W   $��10q�5   5   ��6   �� *
ValueItems+65   �� 5   
   $��1��6   $��0��6"   $��2��62   .+�5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��Update��6   $��25��6*   $	��110��66   $
��Left=�5      $	��Top=�60   $��	10,10,0,0q�6H   ��5      �� *UpdateCommand+�� 5       $��Cancel��6   $��25��6*   $	��110��66   $
��Left=�5!      $	��Top=�60   $��	10,10,0,0q�6H   ��5"      �� *CancelCommand+5#      � 5$      $��15��5%      $��0,5,0,0q�6&    � IsIndeterminate 5$      �� *ShowPreLoader+$ PropertyChanged=�5%   &   5&      5'      5(      