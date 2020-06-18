;#DropDB SBT.Apps.medicoEasyTest

#Application SBT.Apps.Medico.Win
#Application SBT.Apps.Medico.Web

*FillForm
 User Name = Admin
*Action Log On

#IfDef SBT.Apps.Medico.Win

*Action About...
 
*Action OK

#EndIf

#IfDef SBT.Apps.Medico.Web

*Action Choose Theme(Black Glass)

#EndIf
