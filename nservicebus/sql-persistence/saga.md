---
title: Saga Persister
component: SqlPersistence
tags:
 - Persistence
related:
 - samples/sql-persistence
reviewed: 2016-11-29
---



## Json.net Settings


### Custom Settings

Customizes the instance of [JsonSerializerSettings](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonSerializerSettings.htm) used for serialization.

snippet: SqlPersistenceCustomSettings


#### Version / Type specific deserialization settings

The Type and Saga Assembly version are persisted. It is possible to explicitly control the deserialization of sagas based on Version and/or Type. This allows the serialization approach to be evolved forward while avoiding migrations.

snippet: SqlPersistenceJsonSettingsForVersion


### Custom Reader

Customize the creation of the [JsonReader](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonReader.htm).

snippet:SqlPersistenceCustomReader


### Custom Writer

Customize the creation of the [JsonWriter](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonWriter.htm).

snippet:SqlPersistenceCustomWriter


## Saga Definition

Sagas need to be decorated with a `[SqlSagaAttribute]`. If no [Saga Finder](/nservicebus/sagas/saga-finding.md) is defined then the `correlationProperty` needs to match the [Correlated Saga Property](/nservicebus/sagas/message-correlation.md).

snippet: SqlPersistenceSaga


### SqlSaga

`SqlSaga<T>` is an extension of `Saga<T>` that has a less verbose mapping API. The `ToSaga` part is inferred from the `[SqlSagaAttribute]`.

snippet: SqlPersistenceSqlSaga
