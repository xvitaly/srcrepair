/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2019 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2019 EasyCoding Team.
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
*/
namespace srcrepair.core
{
    public interface IType1Settings
    {
        string AntiAliasing { get; }
        string AntiAliasingMSAA { get; }
        string AntiAliasQuality { get; }
        string AntiAliasQualityMSAA { get; }
        string ColorCorrection { get; }
        string DirectXMode { get; }
        string DisplayMode { get; }
        string FilteringMode { get; }
        string FilteringTrilinear { get; }
        string HDRMode { get; }
        string ModelDetail { get; }
        string MotionBlur { get; }
        string ScreenHeight { get; }
        string ScreenWidth { get; }
        string ShaderDetail { get; }
        string ShadowDetail { get; }
        string TextureDetail { get; }
        string VSync { get; }
        string WaterDetail { get; }
        string WaterReflections { get; }
    }
}
