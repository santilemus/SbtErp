update [AuditDataItemPersistent]
set
  GCRecord = 1, AuditedObject = null
where ModifiedOn < '20230430'

update [XPWeakReference]
set
  GCRecord = 1
where Oid in (select NewObject from AuditDataItemPersistent where GCRecord is not null) or
   Oid in (select OldObject from AuditDataItemPersistent where GCRecord is not null) or
   (Oid in (select Oid from AuditedObjectWeakReference) and
    not (Oid in (select AuditedObject from AuditDataItemPersistent where AuditedObject is not null)))

delete from AuditDataItemPersistent
where GCRecord is not null

delete from AuditedObjectWeakReference
where Oid in (select Oid from [XPWeakReference] where GCRecord is not null)

delete from [XPWeakReference]
where GCRecord is not null