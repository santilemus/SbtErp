;#DropDB SBT.Apps.ErpEasyTest

#Application SBT.Apps.ErpWin
#Application SBT.Apps.ErpWeb

*FillForm
 User Name = Admin
*Action Log In

#IfDef SBT.Apps.ErpWin

*Action About...
 
*Action OK

#EndIf

#IfDef SBT.Apps.ErpWeb
*Action Navigation(My Details)
#EndIf
