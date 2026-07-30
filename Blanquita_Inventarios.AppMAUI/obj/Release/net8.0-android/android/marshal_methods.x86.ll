; ModuleID = 'marshal_methods.x86.ll'
source_filename = "marshal_methods.x86.ll"
target datalayout = "e-m:e-p:32:32-p270:32:32-p271:32:32-p272:64:64-f64:32:64-f80:32-n8:16:32-S128"
target triple = "i686-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [166 x ptr] zeroinitializer, align 4

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [332 x i32] [
	i32 10166715, ; 0: System.Net.NameResolution.dll => 0x9b21bb => 127
	i32 34715100, ; 1: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 90
	i32 39109920, ; 2: Newtonsoft.Json.dll => 0x254c520 => 54
	i32 40744412, ; 3: Xamarin.AndroidX.Camera.Lifecycle.dll => 0x26db5dc => 65
	i32 42639949, ; 4: System.Threading.Thread => 0x28aa24d => 151
	i32 67008169, ; 5: zh-Hant\Microsoft.Maui.Controls.resources => 0x3fe76a9 => 33
	i32 72070932, ; 6: Microsoft.Maui.Graphics.dll => 0x44bb714 => 53
	i32 117431740, ; 7: System.Runtime.InteropServices => 0x6ffddbc => 141
	i32 165246403, ; 8: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 68
	i32 182336117, ; 9: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 86
	i32 195452805, ; 10: vi/Microsoft.Maui.Controls.resources.dll => 0xba65f85 => 30
	i32 199333315, ; 11: zh-HK/Microsoft.Maui.Controls.resources.dll => 0xbe195c3 => 31
	i32 205061960, ; 12: System.ComponentModel => 0xc38ff48 => 107
	i32 230752869, ; 13: Microsoft.CSharp.dll => 0xdc10265 => 98
	i32 246610117, ; 14: System.Reflection.Emit.Lightweight => 0xeb2f8c5 => 138
	i32 280992041, ; 15: cs/Microsoft.Maui.Controls.resources.dll => 0x10bf9929 => 2
	i32 317674968, ; 16: vi\Microsoft.Maui.Controls.resources => 0x12ef55d8 => 30
	i32 318968648, ; 17: Xamarin.AndroidX.Activity.dll => 0x13031348 => 61
	i32 336156722, ; 18: ja/Microsoft.Maui.Controls.resources.dll => 0x14095832 => 15
	i32 342366114, ; 19: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 75
	i32 347068432, ; 20: SQLitePCLRaw.lib.e_sqlite3.android.dll => 0x14afd810 => 58
	i32 356389973, ; 21: it/Microsoft.Maui.Controls.resources.dll => 0x153e1455 => 14
	i32 367780167, ; 22: System.IO.Pipes => 0x15ebe147 => 120
	i32 374914964, ; 23: System.Transactions.Local => 0x1658bf94 => 154
	i32 379916513, ; 24: System.Threading.Thread.dll => 0x16a510e1 => 151
	i32 385762202, ; 25: System.Memory.dll => 0x16fe439a => 124
	i32 392610295, ; 26: System.Threading.ThreadPool.dll => 0x1766c1f7 => 152
	i32 395744057, ; 27: _Microsoft.Android.Resource.Designer => 0x17969339 => 34
	i32 435591531, ; 28: sv/Microsoft.Maui.Controls.resources.dll => 0x19f6996b => 26
	i32 442565967, ; 29: System.Collections => 0x1a61054f => 104
	i32 445393611, ; 30: Blanquita_Inventarios.AppMAUI => 0x1a8c2acb => 97
	i32 450948140, ; 31: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 74
	i32 456227837, ; 32: System.Web.HttpUtility.dll => 0x1b317bfd => 155
	i32 459347974, ; 33: System.Runtime.Serialization.Primitives.dll => 0x1b611806 => 145
	i32 465846621, ; 34: mscorlib => 0x1bc4415d => 160
	i32 469710990, ; 35: System.dll => 0x1bff388e => 159
	i32 498788369, ; 36: System.ObjectModel => 0x1dbae811 => 133
	i32 500358224, ; 37: id/Microsoft.Maui.Controls.resources.dll => 0x1dd2dc50 => 13
	i32 503918385, ; 38: fi/Microsoft.Maui.Controls.resources.dll => 0x1e092f31 => 7
	i32 513247710, ; 39: Microsoft.Extensions.Primitives.dll => 0x1e9789de => 47
	i32 530272170, ; 40: System.Linq.Queryable => 0x1f9b4faa => 122
	i32 539058512, ; 41: Microsoft.Extensions.Logging => 0x20216150 => 44
	i32 592146354, ; 42: pt-BR/Microsoft.Maui.Controls.resources.dll => 0x234b6fb2 => 21
	i32 597488923, ; 43: CommunityToolkit.Maui => 0x239cf51b => 36
	i32 627609679, ; 44: Xamarin.AndroidX.CustomView => 0x2568904f => 72
	i32 627931235, ; 45: nl\Microsoft.Maui.Controls.resources => 0x256d7863 => 19
	i32 672442732, ; 46: System.Collections.Concurrent => 0x2814a96c => 100
	i32 683518922, ; 47: System.Net.Security => 0x28bdabca => 130
	i32 688181140, ; 48: ca/Microsoft.Maui.Controls.resources.dll => 0x2904cf94 => 1
	i32 690569205, ; 49: System.Xml.Linq.dll => 0x29293ff5 => 156
	i32 706645707, ; 50: ko/Microsoft.Maui.Controls.resources.dll => 0x2a1e8ecb => 16
	i32 709557578, ; 51: de/Microsoft.Maui.Controls.resources.dll => 0x2a4afd4a => 4
	i32 722857257, ; 52: System.Runtime.Loader.dll => 0x2b15ed29 => 142
	i32 748832960, ; 53: SQLitePCLRaw.batteries_v2 => 0x2ca248c0 => 56
	i32 759454413, ; 54: System.Net.Requests => 0x2d445acd => 129
	i32 762598435, ; 55: System.IO.Pipes.dll => 0x2d745423 => 120
	i32 775189201, ; 56: System.Data.SqlClient.dll => 0x2e3472d1 => 60
	i32 775507847, ; 57: System.IO.Compression => 0x2e394f87 => 119
	i32 777317022, ; 58: sk\Microsoft.Maui.Controls.resources => 0x2e54ea9e => 25
	i32 789151979, ; 59: Microsoft.Extensions.Options => 0x2f0980eb => 46
	i32 804715423, ; 60: System.Data.Common => 0x2ff6fb9f => 110
	i32 823281589, ; 61: System.Private.Uri.dll => 0x311247b5 => 134
	i32 830298997, ; 62: System.IO.Compression.Brotli => 0x317d5b75 => 118
	i32 839353180, ; 63: ZXing.Net.MAUI.Controls.dll => 0x3207835c => 95
	i32 865465478, ; 64: zxing.dll => 0x3395f486 => 93
	i32 876509831, ; 65: Blanquita_Inventarios.Entities.dll => 0x343e7a87 => 96
	i32 904024072, ; 66: System.ComponentModel.Primitives.dll => 0x35e25008 => 105
	i32 926902833, ; 67: tr/Microsoft.Maui.Controls.resources.dll => 0x373f6a31 => 28
	i32 928116545, ; 68: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 90
	i32 955402788, ; 69: Newtonsoft.Json => 0x38f24a24 => 54
	i32 967690846, ; 70: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 75
	i32 975236339, ; 71: System.Diagnostics.Tracing => 0x3a20ecf3 => 114
	i32 975874589, ; 72: System.Xml.XDocument => 0x3a2aaa1d => 158
	i32 992768348, ; 73: System.Collections.dll => 0x3b2c715c => 104
	i32 1012816738, ; 74: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 85
	i32 1019214401, ; 75: System.Drawing => 0x3cbffa41 => 116
	i32 1028951442, ; 76: Microsoft.Extensions.DependencyInjection.Abstractions => 0x3d548d92 => 43
	i32 1029334545, ; 77: da/Microsoft.Maui.Controls.resources.dll => 0x3d5a6611 => 3
	i32 1035644815, ; 78: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 62
	i32 1036536393, ; 79: System.Drawing.Primitives.dll => 0x3dc84a49 => 115
	i32 1044663988, ; 80: System.Linq.Expressions.dll => 0x3e444eb4 => 121
	i32 1052210849, ; 81: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 77
	i32 1082857460, ; 82: System.ComponentModel.TypeConverter => 0x408b17f4 => 106
	i32 1084122840, ; 83: Xamarin.Kotlin.StdLib => 0x409e66d8 => 91
	i32 1098259244, ; 84: System => 0x41761b2c => 159
	i32 1118262833, ; 85: ko\Microsoft.Maui.Controls.resources => 0x42a75631 => 16
	i32 1168523401, ; 86: pt\Microsoft.Maui.Controls.resources => 0x45a64089 => 22
	i32 1178241025, ; 87: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 82
	i32 1203215381, ; 88: pl/Microsoft.Maui.Controls.resources.dll => 0x47b79c15 => 20
	i32 1208641965, ; 89: System.Diagnostics.Process => 0x480a69ad => 112
	i32 1214827643, ; 90: CommunityToolkit.Mvvm => 0x4868cc7b => 38
	i32 1234928153, ; 91: nb/Microsoft.Maui.Controls.resources.dll => 0x499b8219 => 18
	i32 1260983243, ; 92: cs\Microsoft.Maui.Controls.resources => 0x4b2913cb => 2
	i32 1292207520, ; 93: SQLitePCLRaw.core.dll => 0x4d0585a0 => 57
	i32 1293217323, ; 94: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 73
	i32 1324164729, ; 95: System.Linq => 0x4eed2679 => 123
	i32 1373134921, ; 96: zh-Hans\Microsoft.Maui.Controls.resources => 0x51d86049 => 32
	i32 1376866003, ; 97: Xamarin.AndroidX.SavedState => 0x52114ed3 => 85
	i32 1406073936, ; 98: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 69
	i32 1408764838, ; 99: System.Runtime.Serialization.Formatters.dll => 0x53f80ba6 => 144
	i32 1430672901, ; 100: ar\Microsoft.Maui.Controls.resources => 0x55465605 => 0
	i32 1452070440, ; 101: System.Formats.Asn1.dll => 0x568cd628 => 117
	i32 1458022317, ; 102: System.Net.Security.dll => 0x56e7a7ad => 130
	i32 1461004990, ; 103: es\Microsoft.Maui.Controls.resources => 0x57152abe => 6
	i32 1461234159, ; 104: System.Collections.Immutable.dll => 0x5718a9ef => 101
	i32 1462112819, ; 105: System.IO.Compression.dll => 0x57261233 => 119
	i32 1469204771, ; 106: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 63
	i32 1470490898, ; 107: Microsoft.Extensions.Primitives => 0x57a5e912 => 47
	i32 1479771757, ; 108: System.Collections.Immutable => 0x5833866d => 101
	i32 1480492111, ; 109: System.IO.Compression.Brotli.dll => 0x583e844f => 118
	i32 1487239319, ; 110: Microsoft.Win32.Primitives => 0x58a57897 => 99
	i32 1493001747, ; 111: hi/Microsoft.Maui.Controls.resources.dll => 0x58fd6613 => 10
	i32 1514721132, ; 112: el/Microsoft.Maui.Controls.resources.dll => 0x5a48cf6c => 5
	i32 1543031311, ; 113: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 150
	i32 1551623176, ; 114: sk/Microsoft.Maui.Controls.resources.dll => 0x5c7be408 => 25
	i32 1622152042, ; 115: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 79
	i32 1624863272, ; 116: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 88
	i32 1634654947, ; 117: CommunityToolkit.Maui.Core.dll => 0x616edae3 => 37
	i32 1636350590, ; 118: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 71
	i32 1639515021, ; 119: System.Net.Http.dll => 0x61b9038d => 125
	i32 1639986890, ; 120: System.Text.RegularExpressions => 0x61c036ca => 150
	i32 1657153582, ; 121: System.Runtime => 0x62c6282e => 146
	i32 1658251792, ; 122: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 89
	i32 1677501392, ; 123: System.Net.Primitives.dll => 0x63fca3d0 => 128
	i32 1679769178, ; 124: System.Security.Cryptography => 0x641f3e5a => 147
	i32 1711441057, ; 125: SQLitePCLRaw.lib.e_sqlite3.android => 0x660284a1 => 58
	i32 1729485958, ; 126: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 67
	i32 1736233607, ; 127: ro/Microsoft.Maui.Controls.resources.dll => 0x677cd287 => 23
	i32 1743415430, ; 128: ca\Microsoft.Maui.Controls.resources => 0x67ea6886 => 1
	i32 1744735666, ; 129: System.Transactions.Local.dll => 0x67fe8db2 => 154
	i32 1750313021, ; 130: Microsoft.Win32.Primitives.dll => 0x6853a83d => 99
	i32 1763938596, ; 131: System.Diagnostics.TraceSource.dll => 0x69239124 => 113
	i32 1766324549, ; 132: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 86
	i32 1770582343, ; 133: Microsoft.Extensions.Logging.dll => 0x6988f147 => 44
	i32 1776026572, ; 134: System.Core.dll => 0x69dc03cc => 109
	i32 1780572499, ; 135: Mono.Android.Runtime.dll => 0x6a216153 => 164
	i32 1782862114, ; 136: ms\Microsoft.Maui.Controls.resources => 0x6a445122 => 17
	i32 1783641731, ; 137: Blanquita_Inventarios.Entities => 0x6a503683 => 96
	i32 1788241197, ; 138: Xamarin.AndroidX.Fragment => 0x6a96652d => 74
	i32 1793755602, ; 139: he\Microsoft.Maui.Controls.resources => 0x6aea89d2 => 9
	i32 1808609942, ; 140: Xamarin.AndroidX.Loader => 0x6bcd3296 => 79
	i32 1813058853, ; 141: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 91
	i32 1813201214, ; 142: Xamarin.Google.Android.Material => 0x6c13413e => 89
	i32 1818569960, ; 143: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 83
	i32 1824175904, ; 144: System.Text.Encoding.Extensions => 0x6cbab720 => 149
	i32 1824722060, ; 145: System.Runtime.Serialization.Formatters => 0x6cc30c8c => 144
	i32 1828688058, ; 146: Microsoft.Extensions.Logging.Abstractions.dll => 0x6cff90ba => 45
	i32 1842015223, ; 147: uk/Microsoft.Maui.Controls.resources.dll => 0x6dcaebf7 => 29
	i32 1853025655, ; 148: sv\Microsoft.Maui.Controls.resources => 0x6e72ed77 => 26
	i32 1858542181, ; 149: System.Linq.Expressions => 0x6ec71a65 => 121
	i32 1870277092, ; 150: System.Reflection.Primitives => 0x6f7a29e4 => 139
	i32 1875935024, ; 151: fr\Microsoft.Maui.Controls.resources => 0x6fd07f30 => 8
	i32 1910275211, ; 152: System.Collections.NonGeneric.dll => 0x71dc7c8b => 102
	i32 1939592360, ; 153: System.Private.Xml.Linq => 0x739bd4a8 => 135
	i32 1968388702, ; 154: Microsoft.Extensions.Configuration.dll => 0x75533a5e => 40
	i32 2003115576, ; 155: el\Microsoft.Maui.Controls.resources => 0x77651e38 => 5
	i32 2019465201, ; 156: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 77
	i32 2025202353, ; 157: ar/Microsoft.Maui.Controls.resources.dll => 0x78b622b1 => 0
	i32 2045470958, ; 158: System.Private.Xml => 0x79eb68ee => 136
	i32 2055257422, ; 159: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 76
	i32 2066184531, ; 160: de\Microsoft.Maui.Controls.resources => 0x7b277953 => 4
	i32 2070888862, ; 161: System.Diagnostics.TraceSource => 0x7b6f419e => 113
	i32 2079903147, ; 162: System.Runtime.dll => 0x7bf8cdab => 146
	i32 2090596640, ; 163: System.Numerics.Vectors => 0x7c9bf920 => 132
	i32 2103459038, ; 164: SQLitePCLRaw.provider.e_sqlite3.dll => 0x7d603cde => 59
	i32 2127167465, ; 165: System.Console => 0x7ec9ffe9 => 108
	i32 2130260716, ; 166: Blanquita_Inventarios.AppMAUI.dll => 0x7ef932ec => 97
	i32 2142473426, ; 167: System.Collections.Specialized => 0x7fb38cd2 => 103
	i32 2159891885, ; 168: Microsoft.Maui => 0x80bd55ad => 51
	i32 2169148018, ; 169: hu\Microsoft.Maui.Controls.resources => 0x814a9272 => 12
	i32 2181898931, ; 170: Microsoft.Extensions.Options.dll => 0x820d22b3 => 46
	i32 2192057212, ; 171: Microsoft.Extensions.Logging.Abstractions => 0x82a8237c => 45
	i32 2193016926, ; 172: System.ObjectModel.dll => 0x82b6c85e => 133
	i32 2201107256, ; 173: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 92
	i32 2201231467, ; 174: System.Net.Http => 0x8334206b => 125
	i32 2207618523, ; 175: it\Microsoft.Maui.Controls.resources => 0x839595db => 14
	i32 2266799131, ; 176: Microsoft.Extensions.Configuration.Abstractions => 0x871c9c1b => 41
	i32 2270573516, ; 177: fr/Microsoft.Maui.Controls.resources.dll => 0x875633cc => 8
	i32 2279755925, ; 178: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 84
	i32 2295906218, ; 179: System.Net.Sockets => 0x88d8bfaa => 131
	i32 2298471582, ; 180: System.Net.Mail => 0x88ffe49e => 126
	i32 2303942373, ; 181: nb\Microsoft.Maui.Controls.resources => 0x89535ee5 => 18
	i32 2305521784, ; 182: System.Private.CoreLib.dll => 0x896b7878 => 162
	i32 2340441535, ; 183: System.Runtime.InteropServices.RuntimeInformation.dll => 0x8b804dbf => 140
	i32 2353062107, ; 184: System.Net.Primitives => 0x8c40e0db => 128
	i32 2368005991, ; 185: System.Xml.ReaderWriter.dll => 0x8d24e767 => 157
	i32 2371007202, ; 186: Microsoft.Extensions.Configuration => 0x8d52b2e2 => 40
	i32 2395872292, ; 187: id\Microsoft.Maui.Controls.resources => 0x8ece1c24 => 13
	i32 2401565422, ; 188: System.Web.HttpUtility => 0x8f24faee => 155
	i32 2427813419, ; 189: hi\Microsoft.Maui.Controls.resources => 0x90b57e2b => 10
	i32 2435356389, ; 190: System.Console.dll => 0x912896e5 => 108
	i32 2458678730, ; 191: System.Net.Sockets.dll => 0x928c75ca => 131
	i32 2465273461, ; 192: SQLitePCLRaw.batteries_v2.dll => 0x92f11675 => 56
	i32 2471841756, ; 193: netstandard.dll => 0x93554fdc => 161
	i32 2475788418, ; 194: Java.Interop.dll => 0x93918882 => 163
	i32 2480646305, ; 195: Microsoft.Maui.Controls => 0x93dba8a1 => 49
	i32 2538310050, ; 196: System.Reflection.Emit.Lightweight.dll => 0x974b89a2 => 138
	i32 2550873716, ; 197: hr\Microsoft.Maui.Controls.resources => 0x980b3e74 => 11
	i32 2562349572, ; 198: Microsoft.CSharp => 0x98ba5a04 => 98
	i32 2563143864, ; 199: AndHUD.dll => 0x98c678b8 => 35
	i32 2585220780, ; 200: System.Text.Encoding.Extensions.dll => 0x9a1756ac => 149
	i32 2589602615, ; 201: System.Threading.ThreadPool => 0x9a5a3337 => 152
	i32 2593496499, ; 202: pl\Microsoft.Maui.Controls.resources => 0x9a959db3 => 20
	i32 2605712449, ; 203: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 92
	i32 2617129537, ; 204: System.Private.Xml.dll => 0x9bfe3a41 => 136
	i32 2620871830, ; 205: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 71
	i32 2626831493, ; 206: ja\Microsoft.Maui.Controls.resources => 0x9c924485 => 15
	i32 2663698177, ; 207: System.Runtime.Loader => 0x9ec4cf01 => 142
	i32 2664396074, ; 208: System.Xml.XDocument.dll => 0x9ecf752a => 158
	i32 2665622720, ; 209: System.Drawing.Primitives => 0x9ee22cc0 => 115
	i32 2676780864, ; 210: System.Data.Common.dll => 0x9f8c6f40 => 110
	i32 2724373263, ; 211: System.Runtime.Numerics.dll => 0xa262a30f => 143
	i32 2732626843, ; 212: Xamarin.AndroidX.Activity => 0xa2e0939b => 61
	i32 2737747696, ; 213: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 63
	i32 2752995522, ; 214: pt-BR\Microsoft.Maui.Controls.resources => 0xa41760c2 => 21
	i32 2758225723, ; 215: Microsoft.Maui.Controls.Xaml => 0xa4672f3b => 50
	i32 2764765095, ; 216: Microsoft.Maui.dll => 0xa4caf7a7 => 51
	i32 2765824710, ; 217: System.Text.Encoding.CodePages.dll => 0xa4db22c6 => 148
	i32 2778768386, ; 218: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 87
	i32 2785988530, ; 219: th\Microsoft.Maui.Controls.resources => 0xa60ecfb2 => 27
	i32 2801831435, ; 220: Microsoft.Maui.Graphics => 0xa7008e0b => 53
	i32 2806116107, ; 221: es/Microsoft.Maui.Controls.resources.dll => 0xa741ef0b => 6
	i32 2810250172, ; 222: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 69
	i32 2831556043, ; 223: nl/Microsoft.Maui.Controls.resources.dll => 0xa8c61dcb => 19
	i32 2833906405, ; 224: Controls.UserDialogs.Maui => 0xa8e9fae5 => 39
	i32 2853208004, ; 225: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 87
	i32 2861189240, ; 226: Microsoft.Maui.Essentials => 0xaa8a4878 => 52
	i32 2868488919, ; 227: CommunityToolkit.Maui.Core => 0xaaf9aad7 => 37
	i32 2905242038, ; 228: mscorlib.dll => 0xad2a79b6 => 160
	i32 2909740682, ; 229: System.Private.CoreLib => 0xad6f1e8a => 162
	i32 2916838712, ; 230: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 88
	i32 2919462931, ; 231: System.Numerics.Vectors.dll => 0xae037813 => 132
	i32 2959614098, ; 232: System.ComponentModel.dll => 0xb0682092 => 107
	i32 2965157864, ; 233: Xamarin.AndroidX.Camera.View => 0xb0bcb7e8 => 66
	i32 2978675010, ; 234: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 73
	i32 2991449226, ; 235: Xamarin.AndroidX.Camera.Core => 0xb24de48a => 64
	i32 3000842441, ; 236: Xamarin.AndroidX.Camera.View.dll => 0xb2dd38c9 => 66
	i32 3038032645, ; 237: _Microsoft.Android.Resource.Designer.dll => 0xb514b305 => 34
	i32 3047751430, ; 238: Xamarin.AndroidX.Camera.Core.dll => 0xb5a8ff06 => 64
	i32 3057625584, ; 239: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 80
	i32 3059408633, ; 240: Mono.Android.Runtime => 0xb65adef9 => 164
	i32 3059793426, ; 241: System.ComponentModel.Primitives => 0xb660be12 => 105
	i32 3077302341, ; 242: hu/Microsoft.Maui.Controls.resources.dll => 0xb76be845 => 12
	i32 3103600923, ; 243: System.Formats.Asn1 => 0xb8fd311b => 117
	i32 3147165239, ; 244: System.Diagnostics.Tracing.dll => 0xbb95ee37 => 114
	i32 3159123045, ; 245: System.Reflection.Primitives.dll => 0xbc4c6465 => 139
	i32 3178803400, ; 246: Xamarin.AndroidX.Navigation.Fragment.dll => 0xbd78b0c8 => 81
	i32 3215347189, ; 247: zxing => 0xbfa64df5 => 93
	i32 3220365878, ; 248: System.Threading => 0xbff2e236 => 153
	i32 3258312781, ; 249: Xamarin.AndroidX.CardView => 0xc235e84d => 67
	i32 3265493905, ; 250: System.Linq.Queryable.dll => 0xc2a37b91 => 122
	i32 3286373667, ; 251: ZXing.Net.MAUI.dll => 0xc3e21523 => 94
	i32 3286872994, ; 252: SQLite-net.dll => 0xc3e9b3a2 => 55
	i32 3297717902, ; 253: Controls.UserDialogs.Maui.dll => 0xc48f2e8e => 39
	i32 3305363605, ; 254: fi\Microsoft.Maui.Controls.resources => 0xc503d895 => 7
	i32 3316684772, ; 255: System.Net.Requests.dll => 0xc5b097e4 => 129
	i32 3317135071, ; 256: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 72
	i32 3346324047, ; 257: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 82
	i32 3357674450, ; 258: ru\Microsoft.Maui.Controls.resources => 0xc8220bd2 => 24
	i32 3360279109, ; 259: SQLitePCLRaw.core => 0xc849ca45 => 57
	i32 3362522851, ; 260: Xamarin.AndroidX.Core => 0xc86c06e3 => 70
	i32 3366347497, ; 261: Java.Interop => 0xc8a662e9 => 163
	i32 3374999561, ; 262: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 84
	i32 3381016424, ; 263: da\Microsoft.Maui.Controls.resources => 0xc9863768 => 3
	i32 3428513518, ; 264: Microsoft.Extensions.DependencyInjection.dll => 0xcc5af6ee => 42
	i32 3430777524, ; 265: netstandard => 0xcc7d82b4 => 161
	i32 3442543374, ; 266: AndHUD => 0xcd310b0e => 35
	i32 3452344032, ; 267: Microsoft.Maui.Controls.Compatibility.dll => 0xcdc696e0 => 48
	i32 3463511458, ; 268: hr/Microsoft.Maui.Controls.resources.dll => 0xce70fda2 => 11
	i32 3471940407, ; 269: System.ComponentModel.TypeConverter.dll => 0xcef19b37 => 106
	i32 3476120550, ; 270: Mono.Android => 0xcf3163e6 => 165
	i32 3479583265, ; 271: ru/Microsoft.Maui.Controls.resources.dll => 0xcf663a21 => 24
	i32 3484440000, ; 272: ro\Microsoft.Maui.Controls.resources => 0xcfb055c0 => 23
	i32 3509114376, ; 273: System.Xml.Linq => 0xd128d608 => 156
	i32 3580758918, ; 274: zh-HK\Microsoft.Maui.Controls.resources => 0xd56e0b86 => 31
	i32 3608519521, ; 275: System.Linq.dll => 0xd715a361 => 123
	i32 3624195450, ; 276: System.Runtime.InteropServices.RuntimeInformation => 0xd804d57a => 140
	i32 3641597786, ; 277: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 76
	i32 3643446276, ; 278: tr\Microsoft.Maui.Controls.resources => 0xd92a9404 => 28
	i32 3643854240, ; 279: Xamarin.AndroidX.Navigation.Fragment => 0xd930cda0 => 81
	i32 3657292374, ; 280: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd9fdda56 => 41
	i32 3672681054, ; 281: Mono.Android.dll => 0xdae8aa5e => 165
	i32 3676461095, ; 282: Xamarin.AndroidX.Camera.Lifecycle => 0xdb225827 => 65
	i32 3697841164, ; 283: zh-Hant/Microsoft.Maui.Controls.resources.dll => 0xdc68940c => 33
	i32 3724971120, ; 284: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 80
	i32 3732100267, ; 285: System.Net.NameResolution => 0xde7354ab => 127
	i32 3748608112, ; 286: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 111
	i32 3751582913, ; 287: ZXing.Net.MAUI.Controls => 0xdf9c9cc1 => 95
	i32 3754567612, ; 288: SQLitePCLRaw.provider.e_sqlite3 => 0xdfca27bc => 59
	i32 3786282454, ; 289: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 68
	i32 3792276235, ; 290: System.Collections.NonGeneric => 0xe2098b0b => 102
	i32 3800979733, ; 291: Microsoft.Maui.Controls.Compatibility => 0xe28e5915 => 48
	i32 3802395368, ; 292: System.Collections.Specialized.dll => 0xe2a3f2e8 => 103
	i32 3817368567, ; 293: CommunityToolkit.Maui.dll => 0xe3886bf7 => 36
	i32 3823082795, ; 294: System.Security.Cryptography.dll => 0xe3df9d2b => 147
	i32 3834665012, ; 295: System.Data.SqlClient => 0xe4905834 => 60
	i32 3841636137, ; 296: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xe4fab729 => 43
	i32 3842894692, ; 297: ZXing.Net.MAUI => 0xe50deb64 => 94
	i32 3844307129, ; 298: System.Net.Mail.dll => 0xe52378b9 => 126
	i32 3849253459, ; 299: System.Runtime.InteropServices.dll => 0xe56ef253 => 141
	i32 3876362041, ; 300: SQLite-net => 0xe70c9739 => 55
	i32 3889960447, ; 301: zh-Hans/Microsoft.Maui.Controls.resources.dll => 0xe7dc15ff => 32
	i32 3896106733, ; 302: System.Collections.Concurrent.dll => 0xe839deed => 100
	i32 3896760992, ; 303: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 70
	i32 3928044579, ; 304: System.Xml.ReaderWriter => 0xea213423 => 157
	i32 3931092270, ; 305: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 83
	i32 3953953790, ; 306: System.Text.Encoding.CodePages => 0xebac8bfe => 148
	i32 3955647286, ; 307: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 62
	i32 3980434154, ; 308: th/Microsoft.Maui.Controls.resources.dll => 0xed409aea => 27
	i32 3987592930, ; 309: he/Microsoft.Maui.Controls.resources.dll => 0xedadd6e2 => 9
	i32 4003436829, ; 310: System.Diagnostics.Process.dll => 0xee9f991d => 112
	i32 4025784931, ; 311: System.Memory => 0xeff49a63 => 124
	i32 4046471985, ; 312: Microsoft.Maui.Controls.Xaml.dll => 0xf1304331 => 50
	i32 4054681211, ; 313: System.Reflection.Emit.ILGeneration => 0xf1ad867b => 137
	i32 4068434129, ; 314: System.Private.Xml.Linq.dll => 0xf27f60d1 => 135
	i32 4073602200, ; 315: System.Threading.dll => 0xf2ce3c98 => 153
	i32 4094352644, ; 316: Microsoft.Maui.Essentials.dll => 0xf40add04 => 52
	i32 4099507663, ; 317: System.Drawing.dll => 0xf45985cf => 116
	i32 4100113165, ; 318: System.Private.Uri => 0xf462c30d => 134
	i32 4102112229, ; 319: pt/Microsoft.Maui.Controls.resources.dll => 0xf48143e5 => 22
	i32 4125707920, ; 320: ms/Microsoft.Maui.Controls.resources.dll => 0xf5e94e90 => 17
	i32 4126470640, ; 321: Microsoft.Extensions.DependencyInjection => 0xf5f4f1f0 => 42
	i32 4147896353, ; 322: System.Reflection.Emit.ILGeneration.dll => 0xf73be021 => 137
	i32 4150914736, ; 323: uk\Microsoft.Maui.Controls.resources => 0xf769eeb0 => 29
	i32 4151237749, ; 324: System.Core => 0xf76edc75 => 109
	i32 4181436372, ; 325: System.Runtime.Serialization.Primitives => 0xf93ba7d4 => 145
	i32 4182413190, ; 326: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 78
	i32 4213026141, ; 327: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 111
	i32 4271975918, ; 328: Microsoft.Maui.Controls.dll => 0xfea12dee => 49
	i32 4274623895, ; 329: CommunityToolkit.Mvvm.dll => 0xfec99597 => 38
	i32 4274976490, ; 330: System.Runtime.Numerics => 0xfecef6ea => 143
	i32 4292120959 ; 331: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 78
], align 4

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [332 x i32] [
	i32 127, ; 0
	i32 90, ; 1
	i32 54, ; 2
	i32 65, ; 3
	i32 151, ; 4
	i32 33, ; 5
	i32 53, ; 6
	i32 141, ; 7
	i32 68, ; 8
	i32 86, ; 9
	i32 30, ; 10
	i32 31, ; 11
	i32 107, ; 12
	i32 98, ; 13
	i32 138, ; 14
	i32 2, ; 15
	i32 30, ; 16
	i32 61, ; 17
	i32 15, ; 18
	i32 75, ; 19
	i32 58, ; 20
	i32 14, ; 21
	i32 120, ; 22
	i32 154, ; 23
	i32 151, ; 24
	i32 124, ; 25
	i32 152, ; 26
	i32 34, ; 27
	i32 26, ; 28
	i32 104, ; 29
	i32 97, ; 30
	i32 74, ; 31
	i32 155, ; 32
	i32 145, ; 33
	i32 160, ; 34
	i32 159, ; 35
	i32 133, ; 36
	i32 13, ; 37
	i32 7, ; 38
	i32 47, ; 39
	i32 122, ; 40
	i32 44, ; 41
	i32 21, ; 42
	i32 36, ; 43
	i32 72, ; 44
	i32 19, ; 45
	i32 100, ; 46
	i32 130, ; 47
	i32 1, ; 48
	i32 156, ; 49
	i32 16, ; 50
	i32 4, ; 51
	i32 142, ; 52
	i32 56, ; 53
	i32 129, ; 54
	i32 120, ; 55
	i32 60, ; 56
	i32 119, ; 57
	i32 25, ; 58
	i32 46, ; 59
	i32 110, ; 60
	i32 134, ; 61
	i32 118, ; 62
	i32 95, ; 63
	i32 93, ; 64
	i32 96, ; 65
	i32 105, ; 66
	i32 28, ; 67
	i32 90, ; 68
	i32 54, ; 69
	i32 75, ; 70
	i32 114, ; 71
	i32 158, ; 72
	i32 104, ; 73
	i32 85, ; 74
	i32 116, ; 75
	i32 43, ; 76
	i32 3, ; 77
	i32 62, ; 78
	i32 115, ; 79
	i32 121, ; 80
	i32 77, ; 81
	i32 106, ; 82
	i32 91, ; 83
	i32 159, ; 84
	i32 16, ; 85
	i32 22, ; 86
	i32 82, ; 87
	i32 20, ; 88
	i32 112, ; 89
	i32 38, ; 90
	i32 18, ; 91
	i32 2, ; 92
	i32 57, ; 93
	i32 73, ; 94
	i32 123, ; 95
	i32 32, ; 96
	i32 85, ; 97
	i32 69, ; 98
	i32 144, ; 99
	i32 0, ; 100
	i32 117, ; 101
	i32 130, ; 102
	i32 6, ; 103
	i32 101, ; 104
	i32 119, ; 105
	i32 63, ; 106
	i32 47, ; 107
	i32 101, ; 108
	i32 118, ; 109
	i32 99, ; 110
	i32 10, ; 111
	i32 5, ; 112
	i32 150, ; 113
	i32 25, ; 114
	i32 79, ; 115
	i32 88, ; 116
	i32 37, ; 117
	i32 71, ; 118
	i32 125, ; 119
	i32 150, ; 120
	i32 146, ; 121
	i32 89, ; 122
	i32 128, ; 123
	i32 147, ; 124
	i32 58, ; 125
	i32 67, ; 126
	i32 23, ; 127
	i32 1, ; 128
	i32 154, ; 129
	i32 99, ; 130
	i32 113, ; 131
	i32 86, ; 132
	i32 44, ; 133
	i32 109, ; 134
	i32 164, ; 135
	i32 17, ; 136
	i32 96, ; 137
	i32 74, ; 138
	i32 9, ; 139
	i32 79, ; 140
	i32 91, ; 141
	i32 89, ; 142
	i32 83, ; 143
	i32 149, ; 144
	i32 144, ; 145
	i32 45, ; 146
	i32 29, ; 147
	i32 26, ; 148
	i32 121, ; 149
	i32 139, ; 150
	i32 8, ; 151
	i32 102, ; 152
	i32 135, ; 153
	i32 40, ; 154
	i32 5, ; 155
	i32 77, ; 156
	i32 0, ; 157
	i32 136, ; 158
	i32 76, ; 159
	i32 4, ; 160
	i32 113, ; 161
	i32 146, ; 162
	i32 132, ; 163
	i32 59, ; 164
	i32 108, ; 165
	i32 97, ; 166
	i32 103, ; 167
	i32 51, ; 168
	i32 12, ; 169
	i32 46, ; 170
	i32 45, ; 171
	i32 133, ; 172
	i32 92, ; 173
	i32 125, ; 174
	i32 14, ; 175
	i32 41, ; 176
	i32 8, ; 177
	i32 84, ; 178
	i32 131, ; 179
	i32 126, ; 180
	i32 18, ; 181
	i32 162, ; 182
	i32 140, ; 183
	i32 128, ; 184
	i32 157, ; 185
	i32 40, ; 186
	i32 13, ; 187
	i32 155, ; 188
	i32 10, ; 189
	i32 108, ; 190
	i32 131, ; 191
	i32 56, ; 192
	i32 161, ; 193
	i32 163, ; 194
	i32 49, ; 195
	i32 138, ; 196
	i32 11, ; 197
	i32 98, ; 198
	i32 35, ; 199
	i32 149, ; 200
	i32 152, ; 201
	i32 20, ; 202
	i32 92, ; 203
	i32 136, ; 204
	i32 71, ; 205
	i32 15, ; 206
	i32 142, ; 207
	i32 158, ; 208
	i32 115, ; 209
	i32 110, ; 210
	i32 143, ; 211
	i32 61, ; 212
	i32 63, ; 213
	i32 21, ; 214
	i32 50, ; 215
	i32 51, ; 216
	i32 148, ; 217
	i32 87, ; 218
	i32 27, ; 219
	i32 53, ; 220
	i32 6, ; 221
	i32 69, ; 222
	i32 19, ; 223
	i32 39, ; 224
	i32 87, ; 225
	i32 52, ; 226
	i32 37, ; 227
	i32 160, ; 228
	i32 162, ; 229
	i32 88, ; 230
	i32 132, ; 231
	i32 107, ; 232
	i32 66, ; 233
	i32 73, ; 234
	i32 64, ; 235
	i32 66, ; 236
	i32 34, ; 237
	i32 64, ; 238
	i32 80, ; 239
	i32 164, ; 240
	i32 105, ; 241
	i32 12, ; 242
	i32 117, ; 243
	i32 114, ; 244
	i32 139, ; 245
	i32 81, ; 246
	i32 93, ; 247
	i32 153, ; 248
	i32 67, ; 249
	i32 122, ; 250
	i32 94, ; 251
	i32 55, ; 252
	i32 39, ; 253
	i32 7, ; 254
	i32 129, ; 255
	i32 72, ; 256
	i32 82, ; 257
	i32 24, ; 258
	i32 57, ; 259
	i32 70, ; 260
	i32 163, ; 261
	i32 84, ; 262
	i32 3, ; 263
	i32 42, ; 264
	i32 161, ; 265
	i32 35, ; 266
	i32 48, ; 267
	i32 11, ; 268
	i32 106, ; 269
	i32 165, ; 270
	i32 24, ; 271
	i32 23, ; 272
	i32 156, ; 273
	i32 31, ; 274
	i32 123, ; 275
	i32 140, ; 276
	i32 76, ; 277
	i32 28, ; 278
	i32 81, ; 279
	i32 41, ; 280
	i32 165, ; 281
	i32 65, ; 282
	i32 33, ; 283
	i32 80, ; 284
	i32 127, ; 285
	i32 111, ; 286
	i32 95, ; 287
	i32 59, ; 288
	i32 68, ; 289
	i32 102, ; 290
	i32 48, ; 291
	i32 103, ; 292
	i32 36, ; 293
	i32 147, ; 294
	i32 60, ; 295
	i32 43, ; 296
	i32 94, ; 297
	i32 126, ; 298
	i32 141, ; 299
	i32 55, ; 300
	i32 32, ; 301
	i32 100, ; 302
	i32 70, ; 303
	i32 157, ; 304
	i32 83, ; 305
	i32 148, ; 306
	i32 62, ; 307
	i32 27, ; 308
	i32 9, ; 309
	i32 112, ; 310
	i32 124, ; 311
	i32 50, ; 312
	i32 137, ; 313
	i32 135, ; 314
	i32 153, ; 315
	i32 52, ; 316
	i32 116, ; 317
	i32 134, ; 318
	i32 22, ; 319
	i32 17, ; 320
	i32 42, ; 321
	i32 137, ; 322
	i32 29, ; 323
	i32 109, ; 324
	i32 145, ; 325
	i32 78, ; 326
	i32 111, ; 327
	i32 49, ; 328
	i32 38, ; 329
	i32 143, ; 330
	i32 78 ; 331
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 4

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 4

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 4

; Functions

; Function attributes: "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 4, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "stackrealign" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "stackrealign" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" }

; Metadata
!llvm.module.flags = !{!0, !1, !7}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.4xx @ 82d8938cf80f6d5fa6c28529ddfbdb753d805ab4"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"NumRegisterParameters", i32 0}
