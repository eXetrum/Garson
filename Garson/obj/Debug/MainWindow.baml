   M S B A M L   `   `   `  ����A  =Garson, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:GarsonGarson  5   	   5clr-namespace:Garson.PropertiesGarson.Properties  5          Garson.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Garson     5   	   -pclr-namespace:Garson.Properties     5      -   5          � Title$  Garson��5	   	   $	��536��6   $	��757��6%   .��5      � .+�5      �� -   5         ��Name$ buttonEnableSync��5      $��Enable Sync��6%   $
��Left=�6;   $��
10,300,0,0q�6V   $	��Top=�6j   $��75��6�   6�   �� 5      -   5        $ buttonDisableSync��5      $��Disable Sync��6&   {�.  $
��Left=�6O   $��
90,300,0,0q�6j   $	��Top=�6~   $��75��6�   6�   �� 5      -   5         ��Name$ groupBox��5      $��Settings��6   $
��Left=�61   $��	10,10,0,0q�6L   $	��Top=�6_   $	��265��6w   $	��461��6�   .��5      � $��	0,0,-2,-3q�6
   .+�5      �� -   5         ��Name$ textBoxApiKey��5      {�.  $
��Left=�67   $��23��6R   $��
101,12,0,0q�6^    �� TextWrapping$
 Wrap=�6r   $	��Top=�6�   $	��240��6  ��6�   �� *ApiKey+ �� Mode$ TwoWay=�   Garson.Properties.Settings   Default �� Source# Z  �� UpdateSourceTrigger$ PropertyChanged=�6  �� 5      -   5        $ textBoxUserName��5      {�.  $
��Left=�69   $��23��6T   $��
101,60,0,0q�6`   $
 Wrap=�6t   $	��Top=�6�   $	��240��6  ��6�   �� *
UserName+$ TwoWay=�# Z $ PropertyChanged=�6  �� 5      -   5        $ textBoxSiteUrl��5      {�.  $
��Left=�68   $��23��6S   $��101,110,0,0q�6_   $
 Wrap=�6t   $	��Top=�6�   $	��240��6  ��6�   �� *	SiteUrl+$ TwoWay=�# Z $ PropertyChanged=�6  �� 5      -   5        $ textBoxFilter��5      {�.  $
��Left=�67   $��23��6R   $��101,160,0,0q�6^   $
 Wrap=�6s   $	��Top=�6�   $	��240��6  ��6�   �� *Filter+$ TwoWay=�# Z $ PropertyChanged=�6  �� 5      -	   5        $ textBoxFolder��5      {�.  $
��Left=�67   $��23��6R   $��101,210,0,0q�6^   $
 Wrap=�6s   $	��Top=�6�   $	��240��6  ��6�   �� *Folder+$ TwoWay=�# Z $ PropertyChanged=�6  �� 5      -
   5        	 ��Name$	 labelApiKey��5      $��Api key��6!   $
��Left=�63   $��	10,10,0,0q�6N   $	��Top=�6a   �� 5      -   5        $	 labelUserName��5      $��Username��6#   $
��Left=�66   $��	10,60,0,0q�6Q   $	��Top=�6d   �� 5      -   5        $	 labelSiteUrl��5      $��SiteUrl��6"   $
��Left=�64   $��
10,110,0,0q�6O   $	��Top=�6c   �� 5      -   5        $	 labelFilter��5      $��Filter��6!   $
��Left=�62   $��
10,160,0,0q�6M   $	��Top=�6a   �� 5      -   5        $	 labelFolder��5      $��Folder��6!   $
��Left=�62   $��
10,210,0,0q�6M   $	��Top=�6a   �� 5      -   5        $ buttonEditApiKey��5      $
��Edit��6'   $
��Left=�66   $��
357,10,0,0q�6Q   $	��Top=�6e   $��75��6}   6�   �� 5       -   5        $ buttonEditUserName��5       $
��Edit��6)   $
��Left=�68   $��
357,60,0,0q�6S   $	��Top=�6g   $��75��6   6�   �� 5!      -   5        $ buttonEditSiteUrl��5!      $
��Edit��6(   $
��Left=�67   $��357,110,0,0q�6R   $	��Top=�6g   $��75��6   6�   �� 5"      -   5        $ buttonEditFilter��5"      $
��Edit��6'   $
��Left=�66   $��357,160,0,0q�6Q   $	��Top=�6f   $��75��6~   6�   �� 5#      -   5        $ buttonEditFolder��5#      $
��Edit��6'   $
��Left=�66   $��357,210,0,0q�6Q   $	��Top=�6f   $��75��6~   6�   5%      5&      �� 5'      $
��Left=�6   $��15��6)   $��
10,281,0,0q�65   $	��Top=�6I   $	��461��6a   �� 5(      -   5        
 ��Name$
 listView��5(   6   $
��Left=�6H   $	��169��6c   $��
10,327,0,0q�6p   $	��Top=�6�   $	��729��6�   ��6   �� *SourceCollection+ �� View 5)      �� 5*      .P�5+      �� $��DateTime��6    �� Width$	 140��6R    �� DisplayMemberBinding 6(   �� *
DateTime+6R   �� 5,      $��	EventType��6   $ 80��6T    6)   �� *	EventType+6T   �� 5-      $��FileName��6   $	 280��6R    6(   �� *
FileName+6R   �� 5.      $��Upload Status��6   $	 100��6[    6-   �� *UploadStatus+6[   �� 5/      $
��Link��6   $	 100��6J    6$   �� *Link+6J   51      53      56      57      