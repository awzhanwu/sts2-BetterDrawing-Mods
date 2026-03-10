<div align="center">

# Better Drawing

[**简体中文**](README_ZH.md)

![Version](https://img.shields.io/badge/Version-0.1.0-blue.svg)
![Game](https://img.shields.io/badge/Slay_The_Spire_2-Mod-red.svg)
![Platform](https://img.shields.io/badge/Platform-Windows%20|%20Godot-lightgrey.svg)

*《杀戮尖塔2》的BetterDrawing模组*

</div>

模组在原版游戏的地图绘画功能基础上增添了更多用户自定义功能，且提供了额外的撤销与高亮玩家标记功能。

## ✨ 核心功能

- 在官方地图绘画工具栏中新增按钮，允许玩家自定义画笔的 **颜色** 与 **线宽** 。
- 在地图绘画页面中，按下 **Ctrl+Z** 可以撤销上一步绘画或橡皮擦。
- 多人模式下，鼠标悬停在某位玩家的状态栏时可以 **高亮** 该玩家的地图绘画。

## 🎮 玩家安装

1. 从 **Releases** 页面下载最新的 **zip** 压缩包。
2. 解压并将内部的 `BetterDrawing` 文件夹整体复制到游戏的 `<Slay the Spire 2>/mods/` 目录下。
3. 启动游戏并在 **Mods** 菜单中勾选启用本模组。

## 🐞 Bugs

- 官方的橡皮擦擦除非当前选择颜色时会出现错误。修复此问题需要覆写官方橡皮擦逻辑，暂不考虑。
  - **不建议**在安装本模组的情况下再使用橡皮擦。
  - 若还要使用，请在使用前使用色盘的**吸管功能**吸取**需要擦除的颜色**。

## ⚠️ 版权声明

本模组使用了《杀戮尖塔》官方的材质。这些资源 **版权归原游戏开发者所有**。  
本模组的 MIT 许可 **仅适用于模组作者所创建的代码和资源**。

## ⚙️ 编译

如果你需要自己编译：
- 将 `BetterDrawing.csproj` 的 `Sts2Dir` 属性设置为你的游戏路径。
- 或编译时使用: `dotnet build -p:Sts2Dir="你的游戏路径"`
