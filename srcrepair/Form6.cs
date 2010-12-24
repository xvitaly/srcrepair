/*
 * Модуль создания своего FPS-конфига программы SRC Repair.
 * 
 * Copyright 2011 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2011 EasyCoding Team.
 * 
 * Лицензия: GPL v3 (см. файл GPL.txt).
 * Лицензия контента: Creative Commons 3.0 BY.
 * 
 * Запрещается использовать этот файл при использовании любой
 * лицензии, отличной от GNU GPL версии 3 и с ней совместимой.
 * 
 * Официальный блог EasyCoding Team: http://www.easycoding.org/
 * Официальная страница проекта: http://www.easycoding.org/projects/srcrepair
 * 
 * Более подробная инфорация о программе в readme.txt,
 * о лицензии - в GPL.txt.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace srcrepair
{
    public partial class frmFPGen : Form
    {
        private CFGEdDelegate CETableAdd;

        public frmFPGen(CFGEdDelegate sender)
        {
            InitializeComponent();
            CETableAdd = sender;
        }

        private void GenerateCFG_Click(object sender, EventArgs e)
        {
            // Здесь начинается основное выполнение...
            if (TextureQuality.Checked) // детализация текстур
            {
                switch (TextureQualityBox.SelectedIndex)
                {
                    case 0: CETableAdd("mat_picmip", "2");
                        break;
                    case 1: CETableAdd("mat_picmip", "1");
                        break;
                }
            }

            if (ModelQuality.Checked) // детализация моделей
            {
                switch (ModelQualityBox.SelectedIndex)
                {
                    case 0: CETableAdd("r_rootlod", "2");
                        CETableAdd("r_lod", "2");
                        break;
                    case 1: CETableAdd("r_rootlod", "1");
                        CETableAdd("r_lod", "1");
                        break;
                }
            }

            if (DisableRagdollPhys.Checked) // физика рэгдоллов
            {
                CETableAdd("cl_ragdoll_physics_enable", "0");
                CETableAdd("cl_ragdoll_fade_time", "0");
                CETableAdd("cl_ragdoll_collide", "0");
                CETableAdd("ragdoll_sleepaftertime", "0");
            }

            if (DisableShadows.Checked) // тени
            {
                CETableAdd("r_shadows", "0");
                CETableAdd("r_shadowrendertotexture", "0");
                CETableAdd("r_shadowmaxrendered", "0");
                CETableAdd("r_shadowlod", "0");
            }

            if (ReduceWaterQuality.Checked) // вода
            {
                CETableAdd("r_waterforceexpensive", "0");
                CETableAdd("r_waterforcereflectentities", "0");
            }

            if (DisableEffects.Checked) // эффекты
            {
                CETableAdd("mat_hdr_level", "0");
                CETableAdd("mat_disable_bloom", "1");
                CETableAdd("mat_motion_blur_forward_enabled", "0");
            }

            if (EnableTrilinearF.Checked) // трилинейная фильтрация
            {
                CETableAdd("mat_forceaniso", "0");
                CETableAdd("mat_trilinear", "1");
            }

            if (DisableDecals.Checked) // декали
            {
                CETableAdd("r_decals", "0");
                CETableAdd("mp_decals", "0");
                CETableAdd("r_drawmodeldecals", "0");
                CETableAdd("r_drawdecals", "0");
                CETableAdd("r_drawbatchdecals", "0");
                CETableAdd("r_spray_lifetime", "0.1");
                CETableAdd("cl_playerspraydisable", "1");
            }

            if (DisableShaderE.Checked) // шейдеры и др.
            {
                CETableAdd("mat_reducefillrate", "1");
                CETableAdd("r_dynamic", "0");
                CETableAdd("r_3dsky", "0");
            }

            if (DisableBreaks.Checked) // повреждения объектов
            {
                CETableAdd("func_break_max_pieces", "0");
                CETableAdd("props_break_max_pieces", "0");
                CETableAdd("props_break_max_pieces_perframe", "0");
            }

            if (EnableHardOpt.Checked) // железные оптимизации
            {
                CETableAdd("r_sse2", "1");
                CETableAdd("r_3dnow", "1");
            }

            if (DisableSmallObjs.Checked) // отключаем мелкие объекты на картах
            {
                CETableAdd("r_drawdetailprops", "0");
                CETableAdd("cl_phys_props_max", "0");
            }

            if (EnablePreload.Checked) // включаем предзагрузку
            {
                CETableAdd("cl_forcepreload", "1");
            }

            if (DisableVSync.Checked) // отключаем вертикальную синхронизацию
            {
                CETableAdd("mat_vsync", "0");
            }

            // Выводим сообщение пользователю...
            MessageBox.Show(frmMainW.RM.GetString("FPW_Success"), "FPS-Config builder plug-in", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Завершаем работу плагина...
            Close();
        }

        private void TextureQuality_CheckedChanged(object sender, EventArgs e)
        {
            TextureQualityBox.Enabled = TextureQuality.Checked;
            TextureQualityBox.SelectedIndex = 0;
        }

        private void ModelQuality_CheckedChanged(object sender, EventArgs e)
        {
            ModelQualityBox.Enabled = ModelQuality.Checked;
            ModelQualityBox.SelectedIndex = 0;
        }
    }
}
