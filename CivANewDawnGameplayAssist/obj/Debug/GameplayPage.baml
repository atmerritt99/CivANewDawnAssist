   M S B A M L   `   `   `  ����T  PCivANewDawnGameplayAssist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:CivANewDawnGameplayAssistCivANewDawnGameplayAssist  5      ,    &CivANewDawnGameplayAssist.GameplayPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:CivANewDawnGameplayAssist     5        N� Title$  GameplayPage��5	      .��5      � 
������� � ColumnDefinitions 5   
   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6    � RowDefinitions 5   
   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   .+�5   
   �� $��1��6   .��6   Current Turn6,   �� 5    
   $��3��6   .��6   Upcoming Events6/   �� 5!   
   -   5         ��Name$ txtblck_event1��5!      $��3��6,   $��1��69   $
��Wrap=�6I   $��2��6]   $��24(�6n   6~   �� 5"   
   -   5        $ txtblck_event2��5"      $��3��6,   $��2��69   $
��Wrap=�6I   $��2��6]   $��24(�6n   6~   �� 5#   
   -   5        $ txtblck_event3��5#      $��3��6,   $��3��69   $
��Wrap=�6I   $��2��6]   $��24(�6n   6~   �� 5$   
   -   5        $ txtblck_event4��5$      $��3��6,   $��4��69   $
��Wrap=�6I   $��2��6]   $��24(�6n   6~   �� 5%   
   -   5        $ txtblck_event5��5%      $��3��6,   $��5��69   $
��Wrap=�6I   $��2��6]   $��24(�6n   6~   �� 5&   
   -   5        $ txtblck_currentTurn��5&      $��1��61   $��2��6>   $��3��6O   $��1��6c   $
��Wrap=�6s   $��24(�6�   6�   �� 5'   
   -   5         ��Name$ btn_nextTurn��5'      $��1��6'   $��4��64   .��6_   	Next Turn6j   �� 5(   
   -   5        $ btn_drawMapTile��5(      $��1��6*   $��6��67   .��6e   Draw Map Tile6t   �� 5)   
   -	   5        $ btn_astronomy��5)      $��1��6(   $��7��65   {�.  .��6s   	Astronomy6~   �� 5*   
   -
   5        $ btn_returnFirstMapTile��5*      $��2��61   $��6��6>   {�.  .��6�   Return First Tile6�   �� 5+   
   -   5        $ btn_returnSecondMapTile��5+      $��2��62   $��7��6?   {�.  .��6�   Return Second Tile6�   �� 5,   
   -   5         ��Name$ lbl_mapTile��5,      $��1��6%   $��5��62   6D   5:      5;      