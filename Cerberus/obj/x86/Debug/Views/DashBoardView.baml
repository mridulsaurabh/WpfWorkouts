   M S B A M L   `   `   `  ����C  ?Cerberus, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:CerberusCerberus  5   	   9!clr-namespace:Cerberus.ConvertersCerberus.Converters  5   	       Cerberus.DashBoardView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   )dboardclr-namespace:Cerberus     5   	   8
converters!clr-namespace:Cerberus.Converters     5   	     � Title$  Cerberus��5   	   $	��750��5   	   $	��950��6   $	��750��6(   $
��1000��65   $��Segoi)�6B    � 	Resources 5      ;   5Cerberus.Converters.NotificationToVisibilityConverter %  !NotificationToVisibilityConverter��%   &	          5	   
   67   5
      .��5      �� ���#3B73B9$��2q�6#   ._�5   
   � -   5        $��dashBoardGrid��5       � RowDefinitions 5      �� 5      $
��.10* �6    �� 5      $
��.06* �6    �� 5      $��* �6    �� 5      $
��.10* �6    �� 5      $
��.06* �6    .+�5      �� $��0��6   ���#3B73B9$��0,0,0,2q�68   ._�5      � $��0��6   .+�5      �� $��40��6   $��40��6'   $��/Images/Logo.png��63   $
��Left=�6M   $��Center=�6h   $��10,0,0,0q�6�   �� 5      -   5        $��Cerberus��5       $��25(�60   $
��Bold"�6>   ���#2F5C94$
��Left=�6e   $��Center=�6�   $��55,0,0,0q�6�    � Cursor$
 Hand��6�   6�   �� 5      -   5        $��transitionTypeComboBox��5      $y�Visible=�6=   $��24��6R   $	��150��6^   $��Right=�6j   $��Center=�6�   $��0,0,20,0q�6�   6�   5      5      �� 5      $��1��6    �� Orientation$ 
Horizontal=�6&   $��RightToLeft=�6?   .+�5      �� -   5        $��defaultViewBtn��5      $��24��6/   $	��140��6;   $��25,0,0,0q�6G   $��Switch to default View��6Y   6z   �� 5      -   5        $��showExperiments��5      $��24��60   $	��130��6<   $��10,0,0,0q�6H   $��Show calculate data��6Z   6x   �� 5      -   5        $��24��5      $	��150��6%   $��10,0,0,0q�61   $��Select sensor modules��6C   6c   �� 5       -   5        $��24��5       $	��150��6%   $��10,0,0,0q�61   $��Arrange sensor modules��6C   6d   5!      &    Cerberus.RegionNavigationControl  5"      -   5          Name$ regionNavigationControl��5"   -   $��2��6N   ���#3B73B9$��0,2,0,2q�5#   +      TransitionType$ SlideAndFade=�6E   � 5%      $��3��6    � ColumnDefinitions 5&      �� 5'      �� 5(      $
��.30* �6'   .+�5*      �� -	   5        $��notifyMessagePopUp��5*   !   $��0��6;   $��40��6K   $	��200��6W   $
��Auto��6f   $��10,0,0,0q�6s   $
��Left=�6�   $��Center=�6�    " NotificationWindowDataTemplate#��[ 5+   !   $
 Hand��6�   ��6c   �� *LatestNotification+y�5,   !   �� *LatestNotification+ �� 	Converter# [  5-   !   �� 5.      -
   5        $��createExperimentButton��5.      $��1��67   $��32��6G   $	��160��6S   $��	Calculate��6_   $��0,0,30,0q�6s   $��Right=�5/      $��Center=�65   6P   50      �� 51      $��4��6   ���#3B73B9$��0,2,0,0q�68   ._�52      � .+�53      �� $ 
Horizontal=�6!   $
��Left=�6:   $��Center=�6U   $
 Hand��6p   y�54      �� *LatestNotification+# [  .+�55      F� -   5        
���  ��$��25��55   *   $��20��66   $��Center=�6A   $
��Left=�6\   $��2q�6w   ��56      � 57   "   	 � FillRule$	 EvenOdd=�60   .��58   &   �� 
 �� RadiusX$
 3Q�68    �� RadiusY$ 4Q�6D    �� Rect$ 7.25,1,6.5,15�6P   @� 59   &    @� Center$ 10.5,20&�66    @� RadiusX$	 2.5Q�6G    @� RadiusY$	 2.5Q�6U   5:   #   5<      �� 5=      $��Center=�6H   $��0,2,0,5q�6c   ��6$   �� *TotalNotifications+6c   �� 5>      -   5        $	��120��5>   $   $��Notifications Received��60   $��Center=�6N   $��1,2,0,5q�6i   6z    ^� ToolTip 5?      _� 5@      $+��%You have received some notifications.��6&   $��5q�6V   
�������6a   5B      �� 5C      -   5        $��DataTimeTextBoxPanel��5C       $	��192��6<    �� TextAlignment$ Right=�5D      $��Right=�62   $��Center=�6N   $��0,0,10,0q�6i   ��5C   H   �� *CurrentSystemTime+ �� Mode$ OneWay=�5D   i   5E      5F      5G      5H      5I      