   M S B A M L   `   `   `  ����I  ENetworkService, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:NetworkServiceNetworkService  5   	   C&clr-namespace:NetworkService.ViewModelNetworkService.ViewModel  5   	   ;"clr-namespace:NetworkService.ViewsNetworkService.Views  5   	       NetworkService.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:NetworkService     5   	   =
viewModels&clr-namespace:NetworkService.ViewModel     5   	   4views"clr-namespace:NetworkService.Views     5   	     � WindowStartupLocation$  CenterScreen=�5	   	    � Title$ 
MainWindow��5   	   $	��890��6   $	��450��6)    � 
ResizeMode$ NoResize=�65   $��15(�6K   
��� � � � DataContext 5      2   ,NetworkService.ViewModel.MainWindowViewModel  5   
   6+    � 	Resources 5       �� DataType7   1NetworkService.ViewModel.NetworkEntitiesViewModel.   (NetworkService.Views.NetworkEntitiesView6   0NetworkService.ViewModel.NetworkDisplayViewModel-   'NetworkService.Views.NetworkDisplayView8   2NetworkService.ViewModel.MeasurementGraphViewModel/   )NetworkService.Views.MeasurementGraphView��%�   (��       5      *, +)(�� 7     5      *, +)(�� n     5      *, +)�� 5   
   "  6   .R�5        6*   5      �� 5   
   "  6   .R�5        6)   5      �� 5   
   "  6   .R�5        6+   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   .+�5#   
   ]� $��0��6   $��5q�6!   .+�5$      o� $	��Top=�6   
�������.@�5%      m� $��Network Service View��6    m� CommandParameter$ NetworkEntitiesView��6X   $��13(�6    m� Command 69   �� *
NavCommand+6�   m� 5&      $��Network Grid View��6   $ NetworkDisplayView��6U   $��13(�6{    66   �� *
NavCommand+6�   m� 5'      $��Network Graph View��6   $ MeasurementGraphView��6V   $��13(�6~    67   �� *
NavCommand+6�   5(      5)      � 5*   
   $��1��6   .+�5+      �� ��6   �� *CurrentViewModel+6D   5,      �� 5.   
   -   5        $��
HomeButton��51      
�������$��2��50      $��50��52      $��50��53      $��Center=�54      	 � Cursor$
	 Hand��55      $��10,770,80,10q�57      $��NetworkEntitiesView��58      ��56      �� *
NavCommand+.��59      �� $��Stretch=�6   $��Stretch=�67   .+�5:      �� $��/Icons/home.jpg��6   $��37��61   $��37��6>   $��Center=�6I   5;      5<      �� 5>   
   -   5        $��
UndoButton��5A      
�������$��2��5@      $��50��5B      $��50��5C      $��Center=�5D      $
	 Hand��5E      $��30,770,0,10q�5G      ��5F      �� *UndoCommand+.��5H      �� $��Stretch=�6   $��Stretch=�67   .+�5I      �� $��/Icons/undo.jpg��6   $��37��61   $��37��6>   $��Center=�6I   5J      5K      5M      5N      