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
    public interface IType2Video
    {
        int AntiAliasing { get; set; }
        int Effects { get; set; }
        int FilteringMode { get; set; }
        int MemoryPool { get; set; }
        int ModelQuality { get; set; }
        int MotionBlur { get; set; }
        int RenderingMode { get; set; }
        string ScreenGamma { get; set; }
        int ScreenHeight { get; set; }
        int ScreenMode { get; set; }
        int ScreenRatio { get; set; }
        int ScreenWidth { get; set; }
        int ShaderEffects { get; set; }
        int ShadowQuality { get; set; }
        int VSync { get; set; }

        void WriteSettings();
    }
}
