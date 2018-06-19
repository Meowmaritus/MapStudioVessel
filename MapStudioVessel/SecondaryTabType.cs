using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowsBetterParamEditor
{
    public enum SecondaryTabType
    {
        MODEL_PARAM_ST = 100,

        EVENT_PARAM_ST_Lights = 200,
        EVENT_PARAM_ST_Sounds = 201,
        EVENT_PARAM_ST_SFX = 202,
        EVENT_PARAM_ST_WindSFX = 203,
        EVENT_PARAM_ST_Treasures = 204,
        EVENT_PARAM_ST_Generators = 205,
        EVENT_PARAM_ST_BloodMsg = 206,
        EVENT_PARAM_ST_ObjActs = 207,
        EVENT_PARAM_ST_SpawnPoints = 208,
        EVENT_PARAM_ST_MapOffset = 209,
        EVENT_PARAM_ST_Navimesh = 210,
        EVENT_PARAM_ST_Environment = 211,
        EVENT_PARAM_ST_BlackEyeOrbInvasions = 212,

        POINT_PARAM_ST_Points = 300,
        POINT_PARAM_ST_Spheres = 302,
        POINT_PARAM_ST_Cylinders = 303,
        POINT_PARAM_ST_Boxes = 305,

        PARTS_PARAM_ST_MapPieces = 400,
        PARTS_PARAM_ST_Objects = 401,
        PARTS_PARAM_ST_NPCs = 402,
        PARTS_PARAM_ST_Players = 404,
        PARTS_PARAM_ST_Collisions = 405,
        PARTS_PARAM_ST_Navimeshes = 408,
        PARTS_PARAM_ST_UnusedObjects = 409,
        PARTS_PARAM_ST_UnusedNPCs = 410,
        PARTS_PARAM_ST_UnusedCollisions = 411,
    }
}
