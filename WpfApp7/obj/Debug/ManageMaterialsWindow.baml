   M S B A M L   `   `   `  ����  WpfApp7#    WpfApp7.ManageMaterialsWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$1  +Управление материалами��5   	   $	��696��6(   $	��782��65   ��5      �� 5   
    �� 
StartPoint$	 0,0&�6    �� EndPoint$	 1,1&�6/   .��5      �  � Color$ 	#FF9CCDED��6    � Offset$	 0.0Q�6-   � 5      $ 	#FFA7C5AD��6   $	 1.0Q�6-   5	      .��5      � .+�5   
   �� $��Center=�6   $��Center=�60   $	��545��6M   .+�5      �� -   5         ��Name$ EmployeeInfoTextBlock��5      $��20(�5      $
��Bold"�5      $��Center=�5      $��0,0,0,5q�5      ���#3333335      �� 5      -   5        $ EmployeePositionTextBlock��5      $��16(�5      $��Center=�5      $��0,0,0,20q�5      ���#6666665      �� 5      $1��+Управление материалами��6   $��24(�5      $
��Bold"�5      $��Center=�5      $��0,0,0,20q�5      ���#3333335      �� 5!      $	��200��6   $��10q�6(   .��5"      �� -   5         ��Name$ MaterialsListView��5"      $	��525��66    �� View 5#      �� 5$      .P�5%      �� $��ID��6-    �� Width$ 50��6]   	 �� DisplayMemberBinding	 69   �� *Id+6]   �� 5&      $��Название��6-   $	 200��6e   	 6?   �� *Name+6e   �� 5'      $,��&Количество на складе��6-   $	 150��6|   	 6K   �� *QuantityInStock+6|   5(      5*      5+      �� 5-      $*��$Название материала:��6   $��14(�63   $��	10,10,0,5q�6A   �� 5.      -   5        
 ��Name$
 NameTextBox��5.      $��10q�6+   $��10q�67   $��16(�6D   ���#FFFFFF���#C0C0C0$��1q�5/   ,   �� 51      $-��'Количество на складе:��6   $��14(�65   $��	10,10,0,5q�6C   �� 52      -   5        $
 QuantityTextBox��52      $��10q�6/   $��10q�6;   $��16(�6H   ���#FFFFFF���#C0C0C0$��1q�53   ,   �� 55       �� Orientation$ 
Horizontal=�6   $��Center=�62   .+�56      �� -   5        $'��!Добавить материал��56      $��
10,20,10,0q�6U   $��10q�57      $��16(�6&   ���#007ACC���#FFFFFF���#005A9C$��1q�59   /    � Cursor$
 Hand��5:      �� 5<      -   5        $'��!Изменить материал��5<      $��
10,20,10,0q�6X   $��10q�5=      $��16(�6&   ���#007ACC���#FFFFFF���#005A9C$��1q�5?   /   $
 Hand��5@      �� 5B      -   5        $%��Удалить материал��5B      $��
10,20,10,0q�6W   $��10q�5C      $��16(�6&   ���#007ACC���#FFFFFF���#005A9C$��1q�5E   0   $
 Hand��5F      5G      5H      �� 5I   
   -	   5         ��Name$ 
BackButton��5I      $��
Назад��6%   $��682,10,10,570q�6N   ���	#FF007ACC$��14(�6�   $
��Bold"�6�   ��6e   Z� ��6{   Z� z�5J      V� 5K      .~�5L      �� �� 5M      �� 5N      T� 5O      5P      5R      5T      5U      