<div align="center"><h1>VCDiffGUI</h1>
<img src="https://raw.githubusercontent.com/ShadowTheHedgehogHacking/VCDiffGUI/main/res/preview.jpg" align="center" />
</div>


### About

A really simple GUI wrapper to VCDiff.Encoders.VcEncoder for making Shadow the Hedgehog xdeltas.
.NET 8 app (self contained) for both Windows and Linux

We used this to create our xdeltas for 2P Shadow / Shadow the Hedgehog: Reloaded / 2P-Reloaded, etc...

### Build from source

`dotnet publish VCDiffGUI -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true -p:TrimMode=CopyUsed`
`dotnet publish VCDiffGUI -c Release -r linux-x64 --self-contained true /p:PublishSingleFile=true -p:TrimMode=CopyUsed`

