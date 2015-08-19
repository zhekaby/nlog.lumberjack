# NLog Lumberjack target
NLog target. Send events (logs, metrics and alerts) using Lumberjack protocol.

##1. Environment configuration
###1.1 Logstash
Configure input Lumberjack plugin for Logstash 
```
input {
  lumberjack {
    port => 5000
    ssl_certificate => "/etc/pki/tls/certs/my.domain.public.key.crt"
    ssl_key => "/etc/pki/tls/private/my.domain.private.key.key"
  }
}
```
###1.2 Install certificate to your system
Install certificate to `Local Machine` store. Select `Automaticaly select the certificate store based on the type of certificate`
###1.3 Copy certificate thumbprint
Double click to `.crt` file, go `Details` tab and copy `Thumbprint` field
##2 NLog target configuration
###2.1 Install package
To install NLog.LumberjackTarget, run the following command in the Package Manager Console
```
PM> Install-Package NLog.LumberjackTarget
```
###2.2 Add NLog section to your `App.config` file
```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="nlog" type="NLog.Config.ConfigSectionHandler, NLog"/>
  </configSections>
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5.2"/>
  </startup>
  
  <nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

    <extensions>
      <add assembly="NLog.Targets.Lumberjack"/>
    </extensions>

    <targets async="true">
      <target name="MyLogstashServer" xsi:type="Lumberjack" host="<YOUR HOST NAME>" Thumbprint="<YOUR HOST CERTIFICATE THUMBPRINT WHITHOUT SPACES>"  />
    </targets>

    <rules>
      <logger name="*" minlevel="Trace" writeTo="MyLogstashServer"/>
    </rules>
    
  </nlog>
</configuration>
```
ex.: 
```
<target name="MyLogstashServer" xsi:type="Lumberjack" host="logs.myserver.com" Thumbprint="03E50C24E29F5AE39CDB83411102861828793D3F"  />
```
