   M S B A M L   `   `   `  ����C  ?Cerberus, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:CerberusCerberus  5      9!clr-namespace:Cerberus.ConvertersCerberus.Converters  5      &     Cerberus.RegionNavigationControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      )dboardclr-namespace:Cerberus     5      8
converters!clr-namespace:Cerberus.Converters     5        � 	Resources  5	         SlideAndFadeIn g� Duration �� TargetProperty t� From	 t� To g� DecelerationRatio W� From	 W� To  SlideAndFadeOut g� AccelerationRatio 
 FadeIn  FadeOut  SlideIn  SlideOut 
 GrowIn  GrowOut  GrowAndFadeIn 	 GrowAndFadeOut 

 FlipIn  FlipOut  FlipAndFadeIn  FlipAndFadeOut 
 SpinIn  SpinOut  SpinAndFadeIn  SpinAndFadeOut3   -Cerberus.Converters.CenterCoordinateConverter  CenterCoordinateConverter��%>  &	        &	 �     &	 �    &	 �    &	 B    &	 �    &	 4    &	 �    &	 �    &		 X    &	
 �    &	 #
    &	 [    &	 �    &	 v    &	 d    &	 *    &	 f    &	 m    �� 5   
   .��5      t� $ 0:0:.75D�6!   $ Margin�64   $ 500,0,-500,0X�6W   $ 0X�6k   $ .9Q�6r   W� 5      $ 0:0:.25D�6   $ Opacity�61   $ 0X�6U   $ 1X�6^   5      �� 5   
   .��5      t� $ 0:0:.5D�6!   $ Margin�63   $ -500,0,500,0X�6V   $ .9Q�6h   W� 5      $ 0:0:.5D�6   $ Opacity�60   $ 0X�6T   5      �� 5   
   .��5      W� $ 0:0:.25D�6   $ Opacity�61   $ 0X�6U   $ 1X�6^   5      �� 5   
   .��5      W� $ 0:0:.5D�6   $ Opacity�60   $ 0X�6T   5      �� 5   
   .��5      t� $ 0:0:.75D�6!   $ Margin�64   $ 500,0,-500,0X�6W   $ 0X�6k   $ .9Q�6r   5       �� 5!   
   .��5"      t� $ 0:0:.5D�6!   $ Margin�63   $ -500,0,500,0X�6V   $ .9Q�6h   5#      �� 5&   
   .��5'      W� $L F(RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)�6   $ 0X�6�   $ 1X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 5(      $L F(RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)�6   $ 0X�6�   $ 1X�6�   $ 0:0:.75D�6�   $ .9Q�6�   5)      �� 5*   
   .��5+      W� $L F(RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)�6   $ 0X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 5,      $L F(RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)�6   $ 0X�6�   $ 0:0:.75D�6�   $ .9Q�6�   5-      �� 50   
   .��51      W� $L F(RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)�6   $ 0X�6�   $ 1X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 52      $L F(RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)�6   $ 0X�6�   $ 1X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 53      $ 0:0:.25D�6   $ Opacity�61   $ 0X�6U   $ 1X�6^   54      �� 55   
   .��56      W� $L F(RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)�6   $ 0X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 57      $L F(RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)�6   $ 0X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 58      $ 0:0:.75D�6   $ Opacity�61   $ 0X�6U   59      �� 5<   
   .��5=      W� $K E(RenderTransform).(TransformGroup.Children)[1].(SkewTransform.AngleX)�6   $
 -100X�6�   $ 0X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 5>      $K E(RenderTransform).(TransformGroup.Children)[1].(SkewTransform.AngleY)�6   $
 -100X�6�   $ 0X�6�   $ 0:0:.75D�6�   $ .9Q�6�   5?      �� 5@   
   .��5A      W� $K E(RenderTransform).(TransformGroup.Children)[1].(SkewTransform.AngleX)�6   $	 100X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 5B      $K E(RenderTransform).(TransformGroup.Children)[1].(SkewTransform.AngleY)�6   $	 100X�6�   $ 0:0:.75D�6�   $ .9Q�6�   5C      �� 5F   
   .��5G      W� $K E(RenderTransform).(TransformGroup.Children)[1].(SkewTransform.AngleX)�6   $
 -100X�6�   $ 0X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 5H      $K E(RenderTransform).(TransformGroup.Children)[1].(SkewTransform.AngleY)�6   $
 -100X�6�   $ 0X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 5I      $ 0:0:.25D�6   $ Opacity�61   $ 0X�6U   $ 1X�6^   5J      �� 5K   
   .��5L      W� $K E(RenderTransform).(TransformGroup.Children)[1].(SkewTransform.AngleX)�6   $	 100X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 5M      $K E(RenderTransform).(TransformGroup.Children)[1].(SkewTransform.AngleY)�6   $	 100X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 5N      $ 0:0:.75D�6   $ Opacity�61   $ 0X�6U   5O      �� 5R   
   .��5S      W� $L F(RenderTransform).(TransformGroup.Children)[2].(RotateTransform.Angle)�6   $
 -360X�6�   $ 0X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 5T      $L F(RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)�6   $ 0X�6�   $ 1X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 5U      $L F(RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)�6   $ 0X�6�   $ 1X�6�   $ 0:0:.75D�6�   $ .9Q�6�   5V      �� 5W   
   .��5X      W� $L F(RenderTransform).(TransformGroup.Children)[2].(RotateTransform.Angle)�6   $	 360X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 5Y      $L F(RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)�6   $ 0X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 5Z      $L F(RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)�6   $ 0X�6�   $ 0:0:.75D�6�   $ .9Q�6�   5[      �� 5^   
   .��5_      W� $L F(RenderTransform).(TransformGroup.Children)[2].(RotateTransform.Angle)�6   $
 -360X�6�   $ 0X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 5`      $L F(RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)�6   $ 0X�6�   $ 1X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 5a      $L F(RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)�6   $ 0X�6�   $ 1X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 5b      $ 0:0:.25D�6   $ Opacity�61   $ 0X�6U   $ 1X�6^   5c      �� 5d   
   .��5e      W� $L F(RenderTransform).(TransformGroup.Children)[2].(RotateTransform.Angle)�6   $	 360X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 5f      $L F(RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)�6   $ 0X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 5g      $L F(RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)�6   $ 0X�6�   $ 0:0:.75D�6�   $ .9Q�6�   W� 5h      $ 0:0:.75D�6   $ Opacity�61   $ 0X�6U   5i        5k   
   6/   5l      .��5m      � .+�5n   
   �� -   5        $��currentPresenter��5n      z�5o      V� 5p      .~�5q      �� 	 �� ScaleX$	 1Q�6%   
 �� ScaleY$
 1Q�60    �� CenterX 5r   
   ��  �� RelativeSource ��  �� AncestorType$
 GridL� �� Mode$ FindAncestor=� �� Path$ ActualWidth� �� 	Converter# [  �� CenterY 5s   
   ��  �� $
 GridL�$ FindAncestor=�$ ActualHeight�# [ �� 5t       �� AngleX$ 0Q�6$    �� AngleY$ 0Q�6/    �� CenterX 5u      ��  �� $
 GridL�$ FindAncestor=�$ ActualWidth�# [  �� CenterY 5v      ��  �� $
 GridL�$ FindAncestor=�$ ActualHeight�# [ �� 5w       �� Angle$ 0Q�6&    �� CenterX 5x      ��  �� $
 GridL�$ FindAncestor=�$ ActualWidth�# [  �� CenterY 5y      ��  �� $
 GridL�$ FindAncestor=�$ ActualHeight�# [ T� 5z       T� X$ 0Q�6)    T� Y$ 0Q�6/   5{      5}      5~      5      