#Application SBT.Apps.Medico.Web
#Application SBT.Apps.Medico.Win

#IncludeFile MedicoLogon.inc

*Action Navigation(Empresas)
*ProcessRecord
 Razon Social = Servicios Medicos, S.A de C.V

#IfDef SBT.Apps.Medico.Web
*Action Edit
#EndIf

#IfDef SBT.Apps.Medico.win
*Action LayoutTab.Teléfonos
#EndIf

*Action Teléfonos.New

*FillForm
*Action Telefono.New
*FillForm
Numero = 503 012345678
Tipo = Fijo
*Action OK


