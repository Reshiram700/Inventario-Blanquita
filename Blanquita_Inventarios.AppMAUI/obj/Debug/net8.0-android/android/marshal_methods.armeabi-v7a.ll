; ModuleID = 'marshal_methods.armeabi-v7a.ll'
source_filename = "marshal_methods.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [335 x ptr] zeroinitializer, align 4

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [664 x i32] [
	i32 2616222, ; 0: System.Net.NetworkInformation.dll => 0x27eb9e => 68
	i32 10166715, ; 1: System.Net.NameResolution.dll => 0x9b21bb => 67
	i32 15721112, ; 2: System.Runtime.Intrinsics.dll => 0xefe298 => 108
	i32 32687329, ; 3: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 246
	i32 34715100, ; 4: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 281
	i32 34839235, ; 5: System.IO.FileSystem.DriveInfo => 0x2139ac3 => 48
	i32 39109920, ; 6: Newtonsoft.Json.dll => 0x254c520 => 194
	i32 39485524, ; 7: System.Net.WebSockets.dll => 0x25a8054 => 80
	i32 40744412, ; 8: Xamarin.AndroidX.Camera.Lifecycle.dll => 0x26db5dc => 217
	i32 42639949, ; 9: System.Threading.Thread => 0x28aa24d => 145
	i32 66541672, ; 10: System.Diagnostics.StackTrace => 0x3f75868 => 30
	i32 67008169, ; 11: zh-Hant\Microsoft.Maui.Controls.resources => 0x3fe76a9 => 325
	i32 68219467, ; 12: System.Security.Cryptography.Primitives => 0x410f24b => 124
	i32 72070932, ; 13: Microsoft.Maui.Graphics.dll => 0x44bb714 => 193
	i32 82292897, ; 14: System.Runtime.CompilerServices.VisualC.dll => 0x4e7b0a1 => 102
	i32 101534019, ; 15: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 264
	i32 117431740, ; 16: System.Runtime.InteropServices => 0x6ffddbc => 107
	i32 120558881, ; 17: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 264
	i32 122350210, ; 18: System.Threading.Channels.dll => 0x74aea82 => 139
	i32 134690465, ; 19: Xamarin.Kotlin.StdLib.Jdk7.dll => 0x80736a1 => 285
	i32 142721839, ; 20: System.Net.WebHeaderCollection => 0x881c32f => 77
	i32 149972175, ; 21: System.Security.Cryptography.Primitives.dll => 0x8f064cf => 124
	i32 159306688, ; 22: System.ComponentModel.Annotations => 0x97ed3c0 => 13
	i32 165246403, ; 23: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 220
	i32 176265551, ; 24: System.ServiceProcess => 0xa81994f => 132
	i32 182336117, ; 25: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 266
	i32 184328833, ; 26: System.ValueTuple.dll => 0xafca281 => 151
	i32 195452805, ; 27: vi/Microsoft.Maui.Controls.resources.dll => 0xba65f85 => 322
	i32 199333315, ; 28: zh-HK/Microsoft.Maui.Controls.resources.dll => 0xbe195c3 => 323
	i32 205061960, ; 29: System.ComponentModel => 0xc38ff48 => 18
	i32 209399409, ; 30: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 214
	i32 220171995, ; 31: System.Diagnostics.Debug => 0xd1f8edb => 26
	i32 230216969, ; 32: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 240
	i32 230752869, ; 33: Microsoft.CSharp.dll => 0xdc10265 => 1
	i32 231409092, ; 34: System.Linq.Parallel => 0xdcb05c4 => 59
	i32 231814094, ; 35: System.Globalization => 0xdd133ce => 42
	i32 246610117, ; 36: System.Reflection.Emit.Lightweight => 0xeb2f8c5 => 91
	i32 261689757, ; 37: Xamarin.AndroidX.ConstraintLayout.dll => 0xf99119d => 223
	i32 276479776, ; 38: System.Threading.Timer.dll => 0x107abf20 => 147
	i32 278686392, ; 39: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 242
	i32 280482487, ; 40: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 239
	i32 280992041, ; 41: cs/Microsoft.Maui.Controls.resources.dll => 0x10bf9929 => 294
	i32 291076382, ; 42: System.IO.Pipes.AccessControl.dll => 0x1159791e => 54
	i32 298918909, ; 43: System.Net.Ping.dll => 0x11d123fd => 69
	i32 317674968, ; 44: vi\Microsoft.Maui.Controls.resources => 0x12ef55d8 => 322
	i32 318968648, ; 45: Xamarin.AndroidX.Activity.dll => 0x13031348 => 205
	i32 321597661, ; 46: System.Numerics => 0x132b30dd => 83
	i32 336156722, ; 47: ja/Microsoft.Maui.Controls.resources.dll => 0x14095832 => 307
	i32 342366114, ; 48: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 241
	i32 347068432, ; 49: SQLitePCLRaw.lib.e_sqlite3.android.dll => 0x14afd810 => 198
	i32 356389973, ; 50: it/Microsoft.Maui.Controls.resources.dll => 0x153e1455 => 306
	i32 360082299, ; 51: System.ServiceModel.Web => 0x15766b7b => 131
	i32 367780167, ; 52: System.IO.Pipes => 0x15ebe147 => 55
	i32 374914964, ; 53: System.Transactions.Local => 0x1658bf94 => 149
	i32 375677976, ; 54: System.Net.ServicePoint.dll => 0x16646418 => 74
	i32 379916513, ; 55: System.Threading.Thread.dll => 0x16a510e1 => 145
	i32 385762202, ; 56: System.Memory.dll => 0x16fe439a => 62
	i32 392610295, ; 57: System.Threading.ThreadPool.dll => 0x1766c1f7 => 146
	i32 395744057, ; 58: _Microsoft.Android.Resource.Designer => 0x17969339 => 331
	i32 403441872, ; 59: WindowsBase => 0x180c08d0 => 165
	i32 435591531, ; 60: sv/Microsoft.Maui.Controls.resources.dll => 0x19f6996b => 318
	i32 441335492, ; 61: Xamarin.AndroidX.ConstraintLayout.Core => 0x1a4e3ec4 => 224
	i32 442565967, ; 62: System.Collections => 0x1a61054f => 12
	i32 445393611, ; 63: Blanquita_Inventarios.AppMAUI => 0x1a8c2acb => 0
	i32 450948140, ; 64: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 237
	i32 451504562, ; 65: System.Security.Cryptography.X509Certificates => 0x1ae969b2 => 125
	i32 456227837, ; 66: System.Web.HttpUtility.dll => 0x1b317bfd => 152
	i32 459347974, ; 67: System.Runtime.Serialization.Primitives.dll => 0x1b611806 => 113
	i32 465846621, ; 68: mscorlib => 0x1bc4415d => 166
	i32 469710990, ; 69: System.dll => 0x1bff388e => 164
	i32 476646585, ; 70: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 239
	i32 486930444, ; 71: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 252
	i32 498788369, ; 72: System.ObjectModel => 0x1dbae811 => 84
	i32 500358224, ; 73: id/Microsoft.Maui.Controls.resources.dll => 0x1dd2dc50 => 305
	i32 503918385, ; 74: fi/Microsoft.Maui.Controls.resources.dll => 0x1e092f31 => 299
	i32 513247710, ; 75: Microsoft.Extensions.Primitives.dll => 0x1e9789de => 187
	i32 526420162, ; 76: System.Transactions.dll => 0x1f6088c2 => 150
	i32 527452488, ; 77: Xamarin.Kotlin.StdLib.Jdk7 => 0x1f704948 => 285
	i32 530272170, ; 78: System.Linq.Queryable => 0x1f9b4faa => 60
	i32 539058512, ; 79: Microsoft.Extensions.Logging => 0x20216150 => 183
	i32 540030774, ; 80: System.IO.FileSystem.dll => 0x20303736 => 51
	i32 545304856, ; 81: System.Runtime.Extensions => 0x2080b118 => 103
	i32 546455878, ; 82: System.Runtime.Serialization.Xml => 0x20924146 => 114
	i32 549171840, ; 83: System.Globalization.Calendars => 0x20bbb280 => 40
	i32 557405415, ; 84: Jsr305Binding => 0x213954e7 => 278
	i32 569601784, ; 85: Xamarin.AndroidX.Window.Extensions.Core.Core => 0x21f36ef8 => 275
	i32 577335427, ; 86: System.Security.Cryptography.Cng => 0x22697083 => 120
	i32 592146354, ; 87: pt-BR/Microsoft.Maui.Controls.resources.dll => 0x234b6fb2 => 313
	i32 597488923, ; 88: CommunityToolkit.Maui => 0x239cf51b => 174
	i32 601371474, ; 89: System.IO.IsolatedStorage.dll => 0x23d83352 => 52
	i32 605376203, ; 90: System.IO.Compression.FileSystem => 0x24154ecb => 44
	i32 613668793, ; 91: System.Security.Cryptography.Algorithms => 0x2493d7b9 => 119
	i32 627609679, ; 92: Xamarin.AndroidX.CustomView => 0x2568904f => 229
	i32 627931235, ; 93: nl\Microsoft.Maui.Controls.resources => 0x256d7863 => 311
	i32 639843206, ; 94: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 0x26233b86 => 235
	i32 643868501, ; 95: System.Net => 0x2660a755 => 81
	i32 662205335, ; 96: System.Text.Encodings.Web.dll => 0x27787397 => 136
	i32 663517072, ; 97: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 271
	i32 666292255, ; 98: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 212
	i32 672442732, ; 99: System.Collections.Concurrent => 0x2814a96c => 8
	i32 683518922, ; 100: System.Net.Security => 0x28bdabca => 73
	i32 688181140, ; 101: ca/Microsoft.Maui.Controls.resources.dll => 0x2904cf94 => 293
	i32 690569205, ; 102: System.Xml.Linq.dll => 0x29293ff5 => 155
	i32 691348768, ; 103: Xamarin.KotlinX.Coroutines.Android.dll => 0x29352520 => 287
	i32 693804605, ; 104: System.Windows => 0x295a9e3d => 154
	i32 699345723, ; 105: System.Reflection.Emit => 0x29af2b3b => 92
	i32 700284507, ; 106: Xamarin.Jetbrains.Annotations => 0x29bd7e5b => 282
	i32 700358131, ; 107: System.IO.Compression.ZipFile => 0x29be9df3 => 45
	i32 706645707, ; 108: ko/Microsoft.Maui.Controls.resources.dll => 0x2a1e8ecb => 308
	i32 709557578, ; 109: de/Microsoft.Maui.Controls.resources.dll => 0x2a4afd4a => 296
	i32 720511267, ; 110: Xamarin.Kotlin.StdLib.Jdk8 => 0x2af22123 => 286
	i32 722857257, ; 111: System.Runtime.Loader.dll => 0x2b15ed29 => 109
	i32 735137430, ; 112: System.Security.SecureString.dll => 0x2bd14e96 => 129
	i32 748832960, ; 113: SQLitePCLRaw.batteries_v2 => 0x2ca248c0 => 196
	i32 752232764, ; 114: System.Diagnostics.Contracts.dll => 0x2cd6293c => 25
	i32 755313932, ; 115: Xamarin.Android.Glide.Annotations.dll => 0x2d052d0c => 202
	i32 759454413, ; 116: System.Net.Requests => 0x2d445acd => 72
	i32 762598435, ; 117: System.IO.Pipes.dll => 0x2d745423 => 55
	i32 775189201, ; 118: System.Data.SqlClient.dll => 0x2e3472d1 => 200
	i32 775507847, ; 119: System.IO.Compression => 0x2e394f87 => 46
	i32 777317022, ; 120: sk\Microsoft.Maui.Controls.resources => 0x2e54ea9e => 317
	i32 782533833, ; 121: Xamarin.Google.AutoValue.Annotations.dll => 0x2ea484c9 => 277
	i32 789151979, ; 122: Microsoft.Extensions.Options => 0x2f0980eb => 186
	i32 790371945, ; 123: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 0x2f1c1e69 => 230
	i32 804715423, ; 124: System.Data.Common => 0x2ff6fb9f => 22
	i32 807930345, ; 125: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll => 0x302809e9 => 244
	i32 823281589, ; 126: System.Private.Uri.dll => 0x311247b5 => 86
	i32 830298997, ; 127: System.IO.Compression.Brotli => 0x317d5b75 => 43
	i32 832635846, ; 128: System.Xml.XPath.dll => 0x31a103c6 => 160
	i32 834051424, ; 129: System.Net.Quic => 0x31b69d60 => 71
	i32 839353180, ; 130: ZXing.Net.MAUI.Controls.dll => 0x3207835c => 291
	i32 843511501, ; 131: Xamarin.AndroidX.Print => 0x3246f6cd => 257
	i32 865465478, ; 132: zxing.dll => 0x3395f486 => 289
	i32 873119928, ; 133: Microsoft.VisualBasic => 0x340ac0b8 => 3
	i32 876509831, ; 134: Blanquita_Inventarios.Entities.dll => 0x343e7a87 => 326
	i32 877678880, ; 135: System.Globalization.dll => 0x34505120 => 42
	i32 878954865, ; 136: System.Net.Http.Json => 0x3463c971 => 63
	i32 904024072, ; 137: System.ComponentModel.Primitives.dll => 0x35e25008 => 16
	i32 911108515, ; 138: System.IO.MemoryMappedFiles.dll => 0x364e69a3 => 53
	i32 926902833, ; 139: tr/Microsoft.Maui.Controls.resources.dll => 0x373f6a31 => 320
	i32 928116545, ; 140: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 281
	i32 952186615, ; 141: System.Runtime.InteropServices.JavaScript.dll => 0x38c136f7 => 105
	i32 955402788, ; 142: Newtonsoft.Json => 0x38f24a24 => 194
	i32 956575887, ; 143: Xamarin.Kotlin.StdLib.Jdk8.dll => 0x3904308f => 286
	i32 966729478, ; 144: Xamarin.Google.Crypto.Tink.Android => 0x399f1f06 => 279
	i32 967690846, ; 145: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 241
	i32 975236339, ; 146: System.Diagnostics.Tracing => 0x3a20ecf3 => 34
	i32 975874589, ; 147: System.Xml.XDocument => 0x3a2aaa1d => 158
	i32 986514023, ; 148: System.Private.DataContractSerialization.dll => 0x3acd0267 => 85
	i32 987214855, ; 149: System.Diagnostics.Tools => 0x3ad7b407 => 32
	i32 992768348, ; 150: System.Collections.dll => 0x3b2c715c => 12
	i32 994442037, ; 151: System.IO.FileSystem => 0x3b45fb35 => 51
	i32 1001831731, ; 152: System.IO.UnmanagedMemoryStream.dll => 0x3bb6bd33 => 56
	i32 1012816738, ; 153: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 261
	i32 1019214401, ; 154: System.Drawing => 0x3cbffa41 => 36
	i32 1028951442, ; 155: Microsoft.Extensions.DependencyInjection.Abstractions => 0x3d548d92 => 182
	i32 1029334545, ; 156: da/Microsoft.Maui.Controls.resources.dll => 0x3d5a6611 => 295
	i32 1031528504, ; 157: Xamarin.Google.ErrorProne.Annotations.dll => 0x3d7be038 => 280
	i32 1035644815, ; 158: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 210
	i32 1036536393, ; 159: System.Drawing.Primitives.dll => 0x3dc84a49 => 35
	i32 1044663988, ; 160: System.Linq.Expressions.dll => 0x3e444eb4 => 58
	i32 1052210849, ; 161: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 248
	i32 1061503568, ; 162: Xamarin.Google.AutoValue.Annotations => 0x3f454250 => 277
	i32 1067306892, ; 163: GoogleGson => 0x3f9dcf8c => 178
	i32 1082857460, ; 164: System.ComponentModel.TypeConverter => 0x408b17f4 => 17
	i32 1084122840, ; 165: Xamarin.Kotlin.StdLib => 0x409e66d8 => 283
	i32 1098259244, ; 166: System => 0x41761b2c => 164
	i32 1118262833, ; 167: ko\Microsoft.Maui.Controls.resources => 0x42a75631 => 308
	i32 1121599056, ; 168: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll => 0x42da3e50 => 247
	i32 1127624469, ; 169: Microsoft.Extensions.Logging.Debug => 0x43362f15 => 185
	i32 1149092582, ; 170: Xamarin.AndroidX.Window => 0x447dc2e6 => 274
	i32 1168523401, ; 171: pt\Microsoft.Maui.Controls.resources => 0x45a64089 => 314
	i32 1170634674, ; 172: System.Web.dll => 0x45c677b2 => 153
	i32 1175144683, ; 173: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 270
	i32 1178241025, ; 174: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 255
	i32 1203215381, ; 175: pl/Microsoft.Maui.Controls.resources.dll => 0x47b79c15 => 312
	i32 1204270330, ; 176: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 212
	i32 1208641965, ; 177: System.Diagnostics.Process => 0x480a69ad => 29
	i32 1214827643, ; 178: CommunityToolkit.Mvvm => 0x4868cc7b => 176
	i32 1219128291, ; 179: System.IO.IsolatedStorage => 0x48aa6be3 => 52
	i32 1234928153, ; 180: nb/Microsoft.Maui.Controls.resources.dll => 0x499b8219 => 310
	i32 1243150071, ; 181: Xamarin.AndroidX.Window.Extensions.Core.Core.dll => 0x4a18f6f7 => 275
	i32 1253011324, ; 182: Microsoft.Win32.Registry => 0x4aaf6f7c => 5
	i32 1260983243, ; 183: cs\Microsoft.Maui.Controls.resources => 0x4b2913cb => 294
	i32 1264511973, ; 184: Xamarin.AndroidX.Startup.StartupRuntime.dll => 0x4b5eebe5 => 265
	i32 1267360935, ; 185: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 269
	i32 1273260888, ; 186: Xamarin.AndroidX.Collection.Ktx => 0x4be46b58 => 221
	i32 1275534314, ; 187: Xamarin.KotlinX.Coroutines.Android => 0x4c071bea => 287
	i32 1278448581, ; 188: Xamarin.AndroidX.Annotation.Jvm => 0x4c3393c5 => 209
	i32 1292207520, ; 189: SQLitePCLRaw.core.dll => 0x4d0585a0 => 197
	i32 1293217323, ; 190: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 232
	i32 1309188875, ; 191: System.Private.DataContractSerialization => 0x4e08a30b => 85
	i32 1322716291, ; 192: Xamarin.AndroidX.Window.dll => 0x4ed70c83 => 274
	i32 1324164729, ; 193: System.Linq => 0x4eed2679 => 61
	i32 1335329327, ; 194: System.Runtime.Serialization.Json.dll => 0x4f97822f => 112
	i32 1364015309, ; 195: System.IO => 0x514d38cd => 57
	i32 1373134921, ; 196: zh-Hans\Microsoft.Maui.Controls.resources => 0x51d86049 => 324
	i32 1376866003, ; 197: Xamarin.AndroidX.SavedState => 0x52114ed3 => 261
	i32 1379779777, ; 198: System.Resources.ResourceManager => 0x523dc4c1 => 99
	i32 1402170036, ; 199: System.Configuration.dll => 0x53936ab4 => 19
	i32 1406073936, ; 200: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 225
	i32 1408764838, ; 201: System.Runtime.Serialization.Formatters.dll => 0x53f80ba6 => 111
	i32 1411638395, ; 202: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 101
	i32 1422545099, ; 203: System.Runtime.CompilerServices.VisualC => 0x54ca50cb => 102
	i32 1430672901, ; 204: ar\Microsoft.Maui.Controls.resources => 0x55465605 => 292
	i32 1434145427, ; 205: System.Runtime.Handles => 0x557b5293 => 104
	i32 1435222561, ; 206: Xamarin.Google.Crypto.Tink.Android.dll => 0x558bc221 => 279
	i32 1439761251, ; 207: System.Net.Quic.dll => 0x55d10363 => 71
	i32 1452070440, ; 208: System.Formats.Asn1.dll => 0x568cd628 => 38
	i32 1453312822, ; 209: System.Diagnostics.Tools.dll => 0x569fcb36 => 32
	i32 1457743152, ; 210: System.Runtime.Extensions.dll => 0x56e36530 => 103
	i32 1458022317, ; 211: System.Net.Security.dll => 0x56e7a7ad => 73
	i32 1461004990, ; 212: es\Microsoft.Maui.Controls.resources => 0x57152abe => 298
	i32 1461234159, ; 213: System.Collections.Immutable.dll => 0x5718a9ef => 9
	i32 1461719063, ; 214: System.Security.Cryptography.OpenSsl => 0x57201017 => 123
	i32 1462112819, ; 215: System.IO.Compression.dll => 0x57261233 => 46
	i32 1469204771, ; 216: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 211
	i32 1470490898, ; 217: Microsoft.Extensions.Primitives => 0x57a5e912 => 187
	i32 1479771757, ; 218: System.Collections.Immutable => 0x5833866d => 9
	i32 1480492111, ; 219: System.IO.Compression.Brotli.dll => 0x583e844f => 43
	i32 1487239319, ; 220: Microsoft.Win32.Primitives => 0x58a57897 => 4
	i32 1490025113, ; 221: Xamarin.AndroidX.SavedState.SavedState.Ktx.dll => 0x58cffa99 => 262
	i32 1493001747, ; 222: hi/Microsoft.Maui.Controls.resources.dll => 0x58fd6613 => 302
	i32 1514721132, ; 223: el/Microsoft.Maui.Controls.resources.dll => 0x5a48cf6c => 297
	i32 1536373174, ; 224: System.Diagnostics.TextWriterTraceListener => 0x5b9331b6 => 31
	i32 1543031311, ; 225: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 138
	i32 1543355203, ; 226: System.Reflection.Emit.dll => 0x5bfdbb43 => 92
	i32 1550322496, ; 227: System.Reflection.Extensions.dll => 0x5c680b40 => 93
	i32 1551623176, ; 228: sk/Microsoft.Maui.Controls.resources.dll => 0x5c7be408 => 317
	i32 1565862583, ; 229: System.IO.FileSystem.Primitives => 0x5d552ab7 => 49
	i32 1566207040, ; 230: System.Threading.Tasks.Dataflow.dll => 0x5d5a6c40 => 141
	i32 1573704789, ; 231: System.Runtime.Serialization.Json => 0x5dccd455 => 112
	i32 1580037396, ; 232: System.Threading.Overlapped => 0x5e2d7514 => 140
	i32 1582372066, ; 233: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 231
	i32 1592978981, ; 234: System.Runtime.Serialization.dll => 0x5ef2ee25 => 115
	i32 1597949149, ; 235: Xamarin.Google.ErrorProne.Annotations => 0x5f3ec4dd => 280
	i32 1601112923, ; 236: System.Xml.Serialization => 0x5f6f0b5b => 157
	i32 1603525486, ; 237: Microsoft.Maui.Controls.HotReload.Forms.dll => 0x5f93db6e => 327
	i32 1604827217, ; 238: System.Net.WebClient => 0x5fa7b851 => 76
	i32 1618516317, ; 239: System.Net.WebSockets.Client.dll => 0x6078995d => 79
	i32 1622152042, ; 240: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 251
	i32 1622358360, ; 241: System.Dynamic.Runtime => 0x60b33958 => 37
	i32 1624863272, ; 242: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 273
	i32 1634654947, ; 243: CommunityToolkit.Maui.Core.dll => 0x616edae3 => 175
	i32 1635184631, ; 244: Xamarin.AndroidX.Emoji2.ViewsHelper => 0x6176eff7 => 235
	i32 1636350590, ; 245: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 228
	i32 1639515021, ; 246: System.Net.Http.dll => 0x61b9038d => 64
	i32 1639986890, ; 247: System.Text.RegularExpressions => 0x61c036ca => 138
	i32 1641389582, ; 248: System.ComponentModel.EventBasedAsync.dll => 0x61d59e0e => 15
	i32 1657153582, ; 249: System.Runtime => 0x62c6282e => 116
	i32 1658241508, ; 250: Xamarin.AndroidX.Tracing.Tracing.dll => 0x62d6c1e4 => 267
	i32 1658251792, ; 251: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 276
	i32 1670060433, ; 252: Xamarin.AndroidX.ConstraintLayout => 0x638b1991 => 223
	i32 1675553242, ; 253: System.IO.FileSystem.DriveInfo.dll => 0x63dee9da => 48
	i32 1677501392, ; 254: System.Net.Primitives.dll => 0x63fca3d0 => 70
	i32 1678508291, ; 255: System.Net.WebSockets => 0x640c0103 => 80
	i32 1679769178, ; 256: System.Security.Cryptography => 0x641f3e5a => 126
	i32 1691477237, ; 257: System.Reflection.Metadata => 0x64d1e4f5 => 94
	i32 1696967625, ; 258: System.Security.Cryptography.Csp => 0x6525abc9 => 121
	i32 1698840827, ; 259: Xamarin.Kotlin.StdLib.Common => 0x654240fb => 284
	i32 1701541528, ; 260: System.Diagnostics.Debug.dll => 0x656b7698 => 26
	i32 1711441057, ; 261: SQLitePCLRaw.lib.e_sqlite3.android => 0x660284a1 => 198
	i32 1720223769, ; 262: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx => 0x66888819 => 244
	i32 1726116996, ; 263: System.Reflection.dll => 0x66e27484 => 97
	i32 1728033016, ; 264: System.Diagnostics.FileVersionInfo.dll => 0x66ffb0f8 => 28
	i32 1729485958, ; 265: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 219
	i32 1736233607, ; 266: ro/Microsoft.Maui.Controls.resources.dll => 0x677cd287 => 315
	i32 1743415430, ; 267: ca\Microsoft.Maui.Controls.resources => 0x67ea6886 => 293
	i32 1744735666, ; 268: System.Transactions.Local.dll => 0x67fe8db2 => 149
	i32 1746316138, ; 269: Mono.Android.Export => 0x6816ab6a => 169
	i32 1750313021, ; 270: Microsoft.Win32.Primitives.dll => 0x6853a83d => 4
	i32 1758240030, ; 271: System.Resources.Reader.dll => 0x68cc9d1e => 98
	i32 1763938596, ; 272: System.Diagnostics.TraceSource.dll => 0x69239124 => 33
	i32 1765942094, ; 273: System.Reflection.Extensions => 0x6942234e => 93
	i32 1766324549, ; 274: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 266
	i32 1770582343, ; 275: Microsoft.Extensions.Logging.dll => 0x6988f147 => 183
	i32 1776026572, ; 276: System.Core.dll => 0x69dc03cc => 21
	i32 1777075843, ; 277: System.Globalization.Extensions.dll => 0x69ec0683 => 41
	i32 1780572499, ; 278: Mono.Android.Runtime.dll => 0x6a216153 => 170
	i32 1782862114, ; 279: ms\Microsoft.Maui.Controls.resources => 0x6a445122 => 309
	i32 1783641731, ; 280: Blanquita_Inventarios.Entities => 0x6a503683 => 326
	i32 1788241197, ; 281: Xamarin.AndroidX.Fragment => 0x6a96652d => 237
	i32 1793755602, ; 282: he\Microsoft.Maui.Controls.resources => 0x6aea89d2 => 301
	i32 1808609942, ; 283: Xamarin.AndroidX.Loader => 0x6bcd3296 => 251
	i32 1813058853, ; 284: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 283
	i32 1813201214, ; 285: Xamarin.Google.Android.Material => 0x6c13413e => 276
	i32 1818569960, ; 286: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 256
	i32 1818787751, ; 287: Microsoft.VisualBasic.Core => 0x6c687fa7 => 2
	i32 1824175904, ; 288: System.Text.Encoding.Extensions => 0x6cbab720 => 134
	i32 1824722060, ; 289: System.Runtime.Serialization.Formatters => 0x6cc30c8c => 111
	i32 1827303595, ; 290: Microsoft.VisualStudio.DesignTools.TapContract => 0x6cea70ab => 329
	i32 1828688058, ; 291: Microsoft.Extensions.Logging.Abstractions.dll => 0x6cff90ba => 184
	i32 1842015223, ; 292: uk/Microsoft.Maui.Controls.resources.dll => 0x6dcaebf7 => 321
	i32 1847515442, ; 293: Xamarin.Android.Glide.Annotations => 0x6e1ed932 => 202
	i32 1853025655, ; 294: sv\Microsoft.Maui.Controls.resources => 0x6e72ed77 => 318
	i32 1858542181, ; 295: System.Linq.Expressions => 0x6ec71a65 => 58
	i32 1870277092, ; 296: System.Reflection.Primitives => 0x6f7a29e4 => 95
	i32 1875935024, ; 297: fr\Microsoft.Maui.Controls.resources => 0x6fd07f30 => 300
	i32 1879696579, ; 298: System.Formats.Tar.dll => 0x7009e4c3 => 39
	i32 1885316902, ; 299: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 213
	i32 1885918049, ; 300: Microsoft.VisualStudio.DesignTools.TapContract.dll => 0x7068d361 => 329
	i32 1888955245, ; 301: System.Diagnostics.Contracts => 0x70972b6d => 25
	i32 1889954781, ; 302: System.Reflection.Metadata.dll => 0x70a66bdd => 94
	i32 1898237753, ; 303: System.Reflection.DispatchProxy => 0x7124cf39 => 89
	i32 1900610850, ; 304: System.Resources.ResourceManager.dll => 0x71490522 => 99
	i32 1910275211, ; 305: System.Collections.NonGeneric.dll => 0x71dc7c8b => 10
	i32 1939592360, ; 306: System.Private.Xml.Linq => 0x739bd4a8 => 87
	i32 1956758971, ; 307: System.Resources.Writer => 0x74a1c5bb => 100
	i32 1961813231, ; 308: Xamarin.AndroidX.Security.SecurityCrypto.dll => 0x74eee4ef => 263
	i32 1968388702, ; 309: Microsoft.Extensions.Configuration.dll => 0x75533a5e => 179
	i32 1983156543, ; 310: Xamarin.Kotlin.StdLib.Common.dll => 0x7634913f => 284
	i32 1985761444, ; 311: Xamarin.Android.Glide.GifDecoder => 0x765c50a4 => 204
	i32 2003115576, ; 312: el\Microsoft.Maui.Controls.resources => 0x77651e38 => 297
	i32 2011961780, ; 313: System.Buffers.dll => 0x77ec19b4 => 7
	i32 2019465201, ; 314: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 248
	i32 2025202353, ; 315: ar/Microsoft.Maui.Controls.resources.dll => 0x78b622b1 => 292
	i32 2031763787, ; 316: Xamarin.Android.Glide => 0x791a414b => 201
	i32 2045470958, ; 317: System.Private.Xml => 0x79eb68ee => 88
	i32 2055257422, ; 318: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 243
	i32 2060060697, ; 319: System.Windows.dll => 0x7aca0819 => 154
	i32 2066184531, ; 320: de\Microsoft.Maui.Controls.resources => 0x7b277953 => 296
	i32 2070888862, ; 321: System.Diagnostics.TraceSource => 0x7b6f419e => 33
	i32 2079903147, ; 322: System.Runtime.dll => 0x7bf8cdab => 116
	i32 2090596640, ; 323: System.Numerics.Vectors => 0x7c9bf920 => 82
	i32 2103459038, ; 324: SQLitePCLRaw.provider.e_sqlite3.dll => 0x7d603cde => 199
	i32 2117912485, ; 325: Microsoft.VisualStudio.DesignTools.XamlTapContract.dll => 0x7e3cc7a5 => 330
	i32 2127167465, ; 326: System.Console => 0x7ec9ffe9 => 20
	i32 2130260716, ; 327: Blanquita_Inventarios.AppMAUI.dll => 0x7ef932ec => 0
	i32 2142473426, ; 328: System.Collections.Specialized => 0x7fb38cd2 => 11
	i32 2143790110, ; 329: System.Xml.XmlSerializer.dll => 0x7fc7a41e => 162
	i32 2146852085, ; 330: Microsoft.VisualBasic.dll => 0x7ff65cf5 => 3
	i32 2159891885, ; 331: Microsoft.Maui => 0x80bd55ad => 191
	i32 2169148018, ; 332: hu\Microsoft.Maui.Controls.resources => 0x814a9272 => 304
	i32 2181898931, ; 333: Microsoft.Extensions.Options.dll => 0x820d22b3 => 186
	i32 2192057212, ; 334: Microsoft.Extensions.Logging.Abstractions => 0x82a8237c => 184
	i32 2193016926, ; 335: System.ObjectModel.dll => 0x82b6c85e => 84
	i32 2201107256, ; 336: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 288
	i32 2201231467, ; 337: System.Net.Http => 0x8334206b => 64
	i32 2207618523, ; 338: it\Microsoft.Maui.Controls.resources => 0x839595db => 306
	i32 2217644978, ; 339: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 270
	i32 2222056684, ; 340: System.Threading.Tasks.Parallel => 0x8471e4ec => 143
	i32 2244775296, ; 341: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 252
	i32 2252106437, ; 342: System.Xml.Serialization.dll => 0x863c6ac5 => 157
	i32 2256313426, ; 343: System.Globalization.Extensions => 0x867c9c52 => 41
	i32 2265110946, ; 344: System.Security.AccessControl.dll => 0x8702d9a2 => 117
	i32 2266799131, ; 345: Microsoft.Extensions.Configuration.Abstractions => 0x871c9c1b => 180
	i32 2267999099, ; 346: Xamarin.Android.Glide.DiskLruCache.dll => 0x872eeb7b => 203
	i32 2270573516, ; 347: fr/Microsoft.Maui.Controls.resources.dll => 0x875633cc => 300
	i32 2279755925, ; 348: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 259
	i32 2293034957, ; 349: System.ServiceModel.Web.dll => 0x88acefcd => 131
	i32 2295906218, ; 350: System.Net.Sockets => 0x88d8bfaa => 75
	i32 2298471582, ; 351: System.Net.Mail => 0x88ffe49e => 66
	i32 2303942373, ; 352: nb\Microsoft.Maui.Controls.resources => 0x89535ee5 => 310
	i32 2305521784, ; 353: System.Private.CoreLib.dll => 0x896b7878 => 172
	i32 2315684594, ; 354: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 207
	i32 2320631194, ; 355: System.Threading.Tasks.Parallel.dll => 0x8a52059a => 143
	i32 2340441535, ; 356: System.Runtime.InteropServices.RuntimeInformation.dll => 0x8b804dbf => 106
	i32 2344264397, ; 357: System.ValueTuple => 0x8bbaa2cd => 151
	i32 2353062107, ; 358: System.Net.Primitives => 0x8c40e0db => 70
	i32 2368005991, ; 359: System.Xml.ReaderWriter.dll => 0x8d24e767 => 156
	i32 2371007202, ; 360: Microsoft.Extensions.Configuration => 0x8d52b2e2 => 179
	i32 2378619854, ; 361: System.Security.Cryptography.Csp.dll => 0x8dc6dbce => 121
	i32 2383496789, ; 362: System.Security.Principal.Windows.dll => 0x8e114655 => 127
	i32 2395872292, ; 363: id\Microsoft.Maui.Controls.resources => 0x8ece1c24 => 305
	i32 2401565422, ; 364: System.Web.HttpUtility => 0x8f24faee => 152
	i32 2403452196, ; 365: Xamarin.AndroidX.Emoji2.dll => 0x8f41c524 => 234
	i32 2409983638, ; 366: Microsoft.VisualStudio.DesignTools.MobileTapContracts.dll => 0x8fa56e96 => 328
	i32 2421380589, ; 367: System.Threading.Tasks.Dataflow => 0x905355ed => 141
	i32 2423080555, ; 368: Xamarin.AndroidX.Collection.Ktx.dll => 0x906d466b => 221
	i32 2427813419, ; 369: hi\Microsoft.Maui.Controls.resources => 0x90b57e2b => 302
	i32 2435356389, ; 370: System.Console.dll => 0x912896e5 => 20
	i32 2435904999, ; 371: System.ComponentModel.DataAnnotations.dll => 0x9130f5e7 => 14
	i32 2454642406, ; 372: System.Text.Encoding.dll => 0x924edee6 => 135
	i32 2458678730, ; 373: System.Net.Sockets.dll => 0x928c75ca => 75
	i32 2459001652, ; 374: System.Linq.Parallel.dll => 0x92916334 => 59
	i32 2465273461, ; 375: SQLitePCLRaw.batteries_v2.dll => 0x92f11675 => 196
	i32 2465532216, ; 376: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x92f50938 => 224
	i32 2471841756, ; 377: netstandard.dll => 0x93554fdc => 167
	i32 2475788418, ; 378: Java.Interop.dll => 0x93918882 => 168
	i32 2480646305, ; 379: Microsoft.Maui.Controls => 0x93dba8a1 => 189
	i32 2483903535, ; 380: System.ComponentModel.EventBasedAsync => 0x940d5c2f => 15
	i32 2484371297, ; 381: System.Net.ServicePoint => 0x94147f61 => 74
	i32 2490993605, ; 382: System.AppContext.dll => 0x94798bc5 => 6
	i32 2501346920, ; 383: System.Data.DataSetExtensions => 0x95178668 => 23
	i32 2505896520, ; 384: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 246
	i32 2522472828, ; 385: Xamarin.Android.Glide.dll => 0x9659e17c => 201
	i32 2538310050, ; 386: System.Reflection.Emit.Lightweight.dll => 0x974b89a2 => 91
	i32 2550873716, ; 387: hr\Microsoft.Maui.Controls.resources => 0x980b3e74 => 303
	i32 2562349572, ; 388: Microsoft.CSharp => 0x98ba5a04 => 1
	i32 2563143864, ; 389: AndHUD.dll => 0x98c678b8 => 173
	i32 2570120770, ; 390: System.Text.Encodings.Web => 0x9930ee42 => 136
	i32 2581783588, ; 391: Xamarin.AndroidX.Lifecycle.Runtime.Ktx => 0x99e2e424 => 247
	i32 2581819634, ; 392: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 269
	i32 2585220780, ; 393: System.Text.Encoding.Extensions.dll => 0x9a1756ac => 134
	i32 2585805581, ; 394: System.Net.Ping => 0x9a20430d => 69
	i32 2589602615, ; 395: System.Threading.ThreadPool => 0x9a5a3337 => 146
	i32 2593496499, ; 396: pl\Microsoft.Maui.Controls.resources => 0x9a959db3 => 312
	i32 2605712449, ; 397: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 288
	i32 2615233544, ; 398: Xamarin.AndroidX.Fragment.Ktx => 0x9be14c08 => 238
	i32 2616218305, ; 399: Microsoft.Extensions.Logging.Debug.dll => 0x9bf052c1 => 185
	i32 2617129537, ; 400: System.Private.Xml.dll => 0x9bfe3a41 => 88
	i32 2618712057, ; 401: System.Reflection.TypeExtensions.dll => 0x9c165ff9 => 96
	i32 2620871830, ; 402: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 228
	i32 2624644809, ; 403: Xamarin.AndroidX.DynamicAnimation => 0x9c70e6c9 => 233
	i32 2626831493, ; 404: ja\Microsoft.Maui.Controls.resources => 0x9c924485 => 307
	i32 2627185994, ; 405: System.Diagnostics.TextWriterTraceListener.dll => 0x9c97ad4a => 31
	i32 2629843544, ; 406: System.IO.Compression.ZipFile.dll => 0x9cc03a58 => 45
	i32 2633051222, ; 407: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 242
	i32 2663391936, ; 408: Xamarin.Android.Glide.DiskLruCache => 0x9ec022c0 => 203
	i32 2663698177, ; 409: System.Runtime.Loader => 0x9ec4cf01 => 109
	i32 2664396074, ; 410: System.Xml.XDocument.dll => 0x9ecf752a => 158
	i32 2665622720, ; 411: System.Drawing.Primitives => 0x9ee22cc0 => 35
	i32 2676780864, ; 412: System.Data.Common.dll => 0x9f8c6f40 => 22
	i32 2686887180, ; 413: System.Runtime.Serialization.Xml.dll => 0xa026a50c => 114
	i32 2693849962, ; 414: System.IO.dll => 0xa090e36a => 57
	i32 2701096212, ; 415: Xamarin.AndroidX.Tracing.Tracing => 0xa0ff7514 => 267
	i32 2715334215, ; 416: System.Threading.Tasks.dll => 0xa1d8b647 => 144
	i32 2717744543, ; 417: System.Security.Claims => 0xa1fd7d9f => 118
	i32 2719963679, ; 418: System.Security.Cryptography.Cng.dll => 0xa21f5a1f => 120
	i32 2724373263, ; 419: System.Runtime.Numerics.dll => 0xa262a30f => 110
	i32 2732626843, ; 420: Xamarin.AndroidX.Activity => 0xa2e0939b => 205
	i32 2735172069, ; 421: System.Threading.Channels => 0xa30769e5 => 139
	i32 2737747696, ; 422: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 211
	i32 2740948882, ; 423: System.IO.Pipes.AccessControl => 0xa35f8f92 => 54
	i32 2748088231, ; 424: System.Runtime.InteropServices.JavaScript => 0xa3cc7fa7 => 105
	i32 2752995522, ; 425: pt-BR\Microsoft.Maui.Controls.resources => 0xa41760c2 => 313
	i32 2758225723, ; 426: Microsoft.Maui.Controls.Xaml => 0xa4672f3b => 190
	i32 2764765095, ; 427: Microsoft.Maui.dll => 0xa4caf7a7 => 191
	i32 2765824710, ; 428: System.Text.Encoding.CodePages.dll => 0xa4db22c6 => 133
	i32 2770495804, ; 429: Xamarin.Jetbrains.Annotations.dll => 0xa522693c => 282
	i32 2778768386, ; 430: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 272
	i32 2779977773, ; 431: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 0xa5b3182d => 260
	i32 2785988530, ; 432: th\Microsoft.Maui.Controls.resources => 0xa60ecfb2 => 319
	i32 2788224221, ; 433: Xamarin.AndroidX.Fragment.Ktx.dll => 0xa630ecdd => 238
	i32 2801831435, ; 434: Microsoft.Maui.Graphics => 0xa7008e0b => 193
	i32 2803228030, ; 435: System.Xml.XPath.XDocument.dll => 0xa715dd7e => 159
	i32 2806116107, ; 436: es/Microsoft.Maui.Controls.resources.dll => 0xa741ef0b => 298
	i32 2810250172, ; 437: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 225
	i32 2819470561, ; 438: System.Xml.dll => 0xa80db4e1 => 163
	i32 2821205001, ; 439: System.ServiceProcess.dll => 0xa8282c09 => 132
	i32 2821294376, ; 440: Xamarin.AndroidX.ResourceInspection.Annotation => 0xa8298928 => 260
	i32 2824502124, ; 441: System.Xml.XmlDocument => 0xa85a7b6c => 161
	i32 2831556043, ; 442: nl/Microsoft.Maui.Controls.resources.dll => 0xa8c61dcb => 311
	i32 2833906405, ; 443: Controls.UserDialogs.Maui => 0xa8e9fae5 => 177
	i32 2838993487, ; 444: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll => 0xa9379a4f => 249
	i32 2849599387, ; 445: System.Threading.Overlapped.dll => 0xa9d96f9b => 140
	i32 2853208004, ; 446: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 272
	i32 2855708567, ; 447: Xamarin.AndroidX.Transition => 0xaa36a797 => 268
	i32 2861098320, ; 448: Mono.Android.Export.dll => 0xaa88e550 => 169
	i32 2861189240, ; 449: Microsoft.Maui.Essentials => 0xaa8a4878 => 192
	i32 2868488919, ; 450: CommunityToolkit.Maui.Core => 0xaaf9aad7 => 175
	i32 2870099610, ; 451: Xamarin.AndroidX.Activity.Ktx.dll => 0xab123e9a => 206
	i32 2875164099, ; 452: Jsr305Binding.dll => 0xab5f85c3 => 278
	i32 2875220617, ; 453: System.Globalization.Calendars.dll => 0xab606289 => 40
	i32 2884993177, ; 454: Xamarin.AndroidX.ExifInterface => 0xabf58099 => 236
	i32 2887636118, ; 455: System.Net.dll => 0xac1dd496 => 81
	i32 2899753641, ; 456: System.IO.UnmanagedMemoryStream => 0xacd6baa9 => 56
	i32 2900621748, ; 457: System.Dynamic.Runtime.dll => 0xace3f9b4 => 37
	i32 2901442782, ; 458: System.Reflection => 0xacf080de => 97
	i32 2905242038, ; 459: mscorlib.dll => 0xad2a79b6 => 166
	i32 2909740682, ; 460: System.Private.CoreLib => 0xad6f1e8a => 172
	i32 2916838712, ; 461: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 273
	i32 2919462931, ; 462: System.Numerics.Vectors.dll => 0xae037813 => 82
	i32 2921128767, ; 463: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 208
	i32 2936416060, ; 464: System.Resources.Reader => 0xaf06273c => 98
	i32 2940926066, ; 465: System.Diagnostics.StackTrace.dll => 0xaf4af872 => 30
	i32 2942453041, ; 466: System.Xml.XPath.XDocument => 0xaf624531 => 159
	i32 2959614098, ; 467: System.ComponentModel.dll => 0xb0682092 => 18
	i32 2965157864, ; 468: Xamarin.AndroidX.Camera.View => 0xb0bcb7e8 => 218
	i32 2968338931, ; 469: System.Security.Principal.Windows => 0xb0ed41f3 => 127
	i32 2972252294, ; 470: System.Security.Cryptography.Algorithms.dll => 0xb128f886 => 119
	i32 2978675010, ; 471: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 232
	i32 2987532451, ; 472: Xamarin.AndroidX.Security.SecurityCrypto => 0xb21220a3 => 263
	i32 2991449226, ; 473: Xamarin.AndroidX.Camera.Core => 0xb24de48a => 216
	i32 2996846495, ; 474: Xamarin.AndroidX.Lifecycle.Process.dll => 0xb2a03f9f => 245
	i32 3000842441, ; 475: Xamarin.AndroidX.Camera.View.dll => 0xb2dd38c9 => 218
	i32 3016983068, ; 476: Xamarin.AndroidX.Startup.StartupRuntime => 0xb3d3821c => 265
	i32 3023353419, ; 477: WindowsBase.dll => 0xb434b64b => 165
	i32 3024354802, ; 478: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 240
	i32 3038032645, ; 479: _Microsoft.Android.Resource.Designer.dll => 0xb514b305 => 331
	i32 3047751430, ; 480: Xamarin.AndroidX.Camera.Core.dll => 0xb5a8ff06 => 216
	i32 3056245963, ; 481: Xamarin.AndroidX.SavedState.SavedState.Ktx => 0xb62a9ccb => 262
	i32 3057625584, ; 482: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 253
	i32 3059408633, ; 483: Mono.Android.Runtime => 0xb65adef9 => 170
	i32 3059793426, ; 484: System.ComponentModel.Primitives => 0xb660be12 => 16
	i32 3075834255, ; 485: System.Threading.Tasks => 0xb755818f => 144
	i32 3077302341, ; 486: hu/Microsoft.Maui.Controls.resources.dll => 0xb76be845 => 304
	i32 3090735792, ; 487: System.Security.Cryptography.X509Certificates.dll => 0xb838e2b0 => 125
	i32 3099732863, ; 488: System.Security.Claims.dll => 0xb8c22b7f => 118
	i32 3103600923, ; 489: System.Formats.Asn1 => 0xb8fd311b => 38
	i32 3111772706, ; 490: System.Runtime.Serialization => 0xb979e222 => 115
	i32 3121463068, ; 491: System.IO.FileSystem.AccessControl.dll => 0xba0dbf1c => 47
	i32 3124832203, ; 492: System.Threading.Tasks.Extensions => 0xba4127cb => 142
	i32 3132293585, ; 493: System.Security.AccessControl => 0xbab301d1 => 117
	i32 3147165239, ; 494: System.Diagnostics.Tracing.dll => 0xbb95ee37 => 34
	i32 3148237826, ; 495: GoogleGson.dll => 0xbba64c02 => 178
	i32 3159123045, ; 496: System.Reflection.Primitives.dll => 0xbc4c6465 => 95
	i32 3160747431, ; 497: System.IO.MemoryMappedFiles => 0xbc652da7 => 53
	i32 3178803400, ; 498: Xamarin.AndroidX.Navigation.Fragment.dll => 0xbd78b0c8 => 254
	i32 3192346100, ; 499: System.Security.SecureString => 0xbe4755f4 => 129
	i32 3193515020, ; 500: System.Web => 0xbe592c0c => 153
	i32 3204380047, ; 501: System.Data.dll => 0xbefef58f => 24
	i32 3209718065, ; 502: System.Xml.XmlDocument.dll => 0xbf506931 => 161
	i32 3211777861, ; 503: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 231
	i32 3215347189, ; 504: zxing => 0xbfa64df5 => 289
	i32 3217618498, ; 505: Microsoft.VisualStudio.DesignTools.XamlTapContract => 0xbfc8f642 => 330
	i32 3220365878, ; 506: System.Threading => 0xbff2e236 => 148
	i32 3226221578, ; 507: System.Runtime.Handles.dll => 0xc04c3c0a => 104
	i32 3251039220, ; 508: System.Reflection.DispatchProxy.dll => 0xc1c6ebf4 => 89
	i32 3258312781, ; 509: Xamarin.AndroidX.CardView => 0xc235e84d => 219
	i32 3265493905, ; 510: System.Linq.Queryable.dll => 0xc2a37b91 => 60
	i32 3265893370, ; 511: System.Threading.Tasks.Extensions.dll => 0xc2a993fa => 142
	i32 3277815716, ; 512: System.Resources.Writer.dll => 0xc35f7fa4 => 100
	i32 3279906254, ; 513: Microsoft.Win32.Registry.dll => 0xc37f65ce => 5
	i32 3280506390, ; 514: System.ComponentModel.Annotations.dll => 0xc3888e16 => 13
	i32 3286373667, ; 515: ZXing.Net.MAUI.dll => 0xc3e21523 => 290
	i32 3286872994, ; 516: SQLite-net.dll => 0xc3e9b3a2 => 195
	i32 3290767353, ; 517: System.Security.Cryptography.Encoding => 0xc4251ff9 => 122
	i32 3297717902, ; 518: Controls.UserDialogs.Maui.dll => 0xc48f2e8e => 177
	i32 3299363146, ; 519: System.Text.Encoding => 0xc4a8494a => 135
	i32 3303498502, ; 520: System.Diagnostics.FileVersionInfo => 0xc4e76306 => 28
	i32 3305363605, ; 521: fi\Microsoft.Maui.Controls.resources => 0xc503d895 => 299
	i32 3316684772, ; 522: System.Net.Requests.dll => 0xc5b097e4 => 72
	i32 3317135071, ; 523: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 229
	i32 3317144872, ; 524: System.Data => 0xc5b79d28 => 24
	i32 3340431453, ; 525: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 213
	i32 3345895724, ; 526: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 0xc76e512c => 258
	i32 3346324047, ; 527: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 255
	i32 3357674450, ; 528: ru\Microsoft.Maui.Controls.resources => 0xc8220bd2 => 316
	i32 3358260929, ; 529: System.Text.Json => 0xc82afec1 => 137
	i32 3360279109, ; 530: SQLitePCLRaw.core => 0xc849ca45 => 197
	i32 3362336904, ; 531: Xamarin.AndroidX.Activity.Ktx => 0xc8693088 => 206
	i32 3362522851, ; 532: Xamarin.AndroidX.Core => 0xc86c06e3 => 226
	i32 3366347497, ; 533: Java.Interop => 0xc8a662e9 => 168
	i32 3374999561, ; 534: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 259
	i32 3381016424, ; 535: da\Microsoft.Maui.Controls.resources => 0xc9863768 => 295
	i32 3395150330, ; 536: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 101
	i32 3403906625, ; 537: System.Security.Cryptography.OpenSsl.dll => 0xcae37e41 => 123
	i32 3405233483, ; 538: Xamarin.AndroidX.CustomView.PoolingContainer => 0xcaf7bd4b => 230
	i32 3413944578, ; 539: Xamarin.AndroidX.Camera.Camera2.dll => 0xcb7ca902 => 215
	i32 3421910702, ; 540: Xamarin.AndroidX.Camera.Camera2 => 0xcbf636ae => 215
	i32 3428513518, ; 541: Microsoft.Extensions.DependencyInjection.dll => 0xcc5af6ee => 181
	i32 3429136800, ; 542: System.Xml => 0xcc6479a0 => 163
	i32 3430777524, ; 543: netstandard => 0xcc7d82b4 => 167
	i32 3441283291, ; 544: Xamarin.AndroidX.DynamicAnimation.dll => 0xcd1dd0db => 233
	i32 3442543374, ; 545: AndHUD => 0xcd310b0e => 173
	i32 3445260447, ; 546: System.Formats.Tar => 0xcd5a809f => 39
	i32 3452344032, ; 547: Microsoft.Maui.Controls.Compatibility.dll => 0xcdc696e0 => 188
	i32 3463511458, ; 548: hr/Microsoft.Maui.Controls.resources.dll => 0xce70fda2 => 303
	i32 3471940407, ; 549: System.ComponentModel.TypeConverter.dll => 0xcef19b37 => 17
	i32 3476120550, ; 550: Mono.Android => 0xcf3163e6 => 171
	i32 3479583265, ; 551: ru/Microsoft.Maui.Controls.resources.dll => 0xcf663a21 => 316
	i32 3484440000, ; 552: ro\Microsoft.Maui.Controls.resources => 0xcfb055c0 => 315
	i32 3485117614, ; 553: System.Text.Json.dll => 0xcfbaacae => 137
	i32 3486566296, ; 554: System.Transactions => 0xcfd0c798 => 150
	i32 3493954962, ; 555: Xamarin.AndroidX.Concurrent.Futures.dll => 0xd0418592 => 222
	i32 3509114376, ; 556: System.Xml.Linq => 0xd128d608 => 155
	i32 3515174580, ; 557: System.Security.dll => 0xd1854eb4 => 130
	i32 3530912306, ; 558: System.Configuration => 0xd2757232 => 19
	i32 3539954161, ; 559: System.Net.HttpListener => 0xd2ff69f1 => 65
	i32 3560100363, ; 560: System.Threading.Timer => 0xd432d20b => 147
	i32 3570554715, ; 561: System.IO.FileSystem.AccessControl => 0xd4d2575b => 47
	i32 3580758918, ; 562: zh-HK\Microsoft.Maui.Controls.resources => 0xd56e0b86 => 323
	i32 3597029428, ; 563: Xamarin.Android.Glide.GifDecoder.dll => 0xd6665034 => 204
	i32 3598340787, ; 564: System.Net.WebSockets.Client => 0xd67a52b3 => 79
	i32 3608519521, ; 565: System.Linq.dll => 0xd715a361 => 61
	i32 3624195450, ; 566: System.Runtime.InteropServices.RuntimeInformation => 0xd804d57a => 106
	i32 3627220390, ; 567: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 257
	i32 3633644679, ; 568: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 208
	i32 3638274909, ; 569: System.IO.FileSystem.Primitives.dll => 0xd8dbab5d => 49
	i32 3641597786, ; 570: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 243
	i32 3643446276, ; 571: tr\Microsoft.Maui.Controls.resources => 0xd92a9404 => 320
	i32 3643854240, ; 572: Xamarin.AndroidX.Navigation.Fragment => 0xd930cda0 => 254
	i32 3645089577, ; 573: System.ComponentModel.DataAnnotations => 0xd943a729 => 14
	i32 3657292374, ; 574: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd9fdda56 => 180
	i32 3660523487, ; 575: System.Net.NetworkInformation => 0xda2f27df => 68
	i32 3672681054, ; 576: Mono.Android.dll => 0xdae8aa5e => 171
	i32 3676461095, ; 577: Xamarin.AndroidX.Camera.Lifecycle => 0xdb225827 => 217
	i32 3676670898, ; 578: Microsoft.Maui.Controls.HotReload.Forms => 0xdb258bb2 => 327
	i32 3682565725, ; 579: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 214
	i32 3684561358, ; 580: Xamarin.AndroidX.Concurrent.Futures => 0xdb9df1ce => 222
	i32 3697841164, ; 581: zh-Hant/Microsoft.Maui.Controls.resources.dll => 0xdc68940c => 325
	i32 3700866549, ; 582: System.Net.WebProxy.dll => 0xdc96bdf5 => 78
	i32 3706696989, ; 583: Xamarin.AndroidX.Core.Core.Ktx.dll => 0xdcefb51d => 227
	i32 3716563718, ; 584: System.Runtime.Intrinsics => 0xdd864306 => 108
	i32 3718780102, ; 585: Xamarin.AndroidX.Annotation => 0xdda814c6 => 207
	i32 3724971120, ; 586: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 253
	i32 3732100267, ; 587: System.Net.NameResolution => 0xde7354ab => 67
	i32 3737834244, ; 588: System.Net.Http.Json.dll => 0xdecad304 => 63
	i32 3748608112, ; 589: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 27
	i32 3751444290, ; 590: System.Xml.XPath => 0xdf9a7f42 => 160
	i32 3751582913, ; 591: ZXing.Net.MAUI.Controls => 0xdf9c9cc1 => 291
	i32 3754567612, ; 592: SQLitePCLRaw.provider.e_sqlite3 => 0xdfca27bc => 199
	i32 3786282454, ; 593: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 220
	i32 3792276235, ; 594: System.Collections.NonGeneric => 0xe2098b0b => 10
	i32 3800979733, ; 595: Microsoft.Maui.Controls.Compatibility => 0xe28e5915 => 188
	i32 3802395368, ; 596: System.Collections.Specialized.dll => 0xe2a3f2e8 => 11
	i32 3817368567, ; 597: CommunityToolkit.Maui.dll => 0xe3886bf7 => 174
	i32 3819260425, ; 598: System.Net.WebProxy => 0xe3a54a09 => 78
	i32 3823082795, ; 599: System.Security.Cryptography.dll => 0xe3df9d2b => 126
	i32 3829621856, ; 600: System.Numerics.dll => 0xe4436460 => 83
	i32 3834665012, ; 601: System.Data.SqlClient => 0xe4905834 => 200
	i32 3841636137, ; 602: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xe4fab729 => 182
	i32 3842894692, ; 603: ZXing.Net.MAUI => 0xe50deb64 => 290
	i32 3844307129, ; 604: System.Net.Mail.dll => 0xe52378b9 => 66
	i32 3849253459, ; 605: System.Runtime.InteropServices.dll => 0xe56ef253 => 107
	i32 3870376305, ; 606: System.Net.HttpListener.dll => 0xe6b14171 => 65
	i32 3873536506, ; 607: System.Security.Principal => 0xe6e179fa => 128
	i32 3875112723, ; 608: System.Security.Cryptography.Encoding.dll => 0xe6f98713 => 122
	i32 3876362041, ; 609: SQLite-net => 0xe70c9739 => 195
	i32 3885497537, ; 610: System.Net.WebHeaderCollection.dll => 0xe797fcc1 => 77
	i32 3885922214, ; 611: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 268
	i32 3888767677, ; 612: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 0xe7c9e2bd => 258
	i32 3889960447, ; 613: zh-Hans/Microsoft.Maui.Controls.resources.dll => 0xe7dc15ff => 324
	i32 3896106733, ; 614: System.Collections.Concurrent.dll => 0xe839deed => 8
	i32 3896760992, ; 615: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 226
	i32 3901907137, ; 616: Microsoft.VisualBasic.Core.dll => 0xe89260c1 => 2
	i32 3920810846, ; 617: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 44
	i32 3921031405, ; 618: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 271
	i32 3928044579, ; 619: System.Xml.ReaderWriter => 0xea213423 => 156
	i32 3930554604, ; 620: System.Security.Principal.dll => 0xea4780ec => 128
	i32 3931092270, ; 621: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 256
	i32 3945713374, ; 622: System.Data.DataSetExtensions.dll => 0xeb2ecede => 23
	i32 3953953790, ; 623: System.Text.Encoding.CodePages => 0xebac8bfe => 133
	i32 3955647286, ; 624: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 210
	i32 3959773229, ; 625: Xamarin.AndroidX.Lifecycle.Process => 0xec05582d => 245
	i32 3980434154, ; 626: th/Microsoft.Maui.Controls.resources.dll => 0xed409aea => 319
	i32 3987592930, ; 627: he/Microsoft.Maui.Controls.resources.dll => 0xedadd6e2 => 301
	i32 4003436829, ; 628: System.Diagnostics.Process.dll => 0xee9f991d => 29
	i32 4015948917, ; 629: Xamarin.AndroidX.Annotation.Jvm.dll => 0xef5e8475 => 209
	i32 4025784931, ; 630: System.Memory => 0xeff49a63 => 62
	i32 4046471985, ; 631: Microsoft.Maui.Controls.Xaml.dll => 0xf1304331 => 190
	i32 4054681211, ; 632: System.Reflection.Emit.ILGeneration => 0xf1ad867b => 90
	i32 4068434129, ; 633: System.Private.Xml.Linq.dll => 0xf27f60d1 => 87
	i32 4073602200, ; 634: System.Threading.dll => 0xf2ce3c98 => 148
	i32 4094352644, ; 635: Microsoft.Maui.Essentials.dll => 0xf40add04 => 192
	i32 4099507663, ; 636: System.Drawing.dll => 0xf45985cf => 36
	i32 4100113165, ; 637: System.Private.Uri => 0xf462c30d => 86
	i32 4101593132, ; 638: Xamarin.AndroidX.Emoji2 => 0xf479582c => 234
	i32 4102112229, ; 639: pt/Microsoft.Maui.Controls.resources.dll => 0xf48143e5 => 314
	i32 4125707920, ; 640: ms/Microsoft.Maui.Controls.resources.dll => 0xf5e94e90 => 309
	i32 4126470640, ; 641: Microsoft.Extensions.DependencyInjection => 0xf5f4f1f0 => 181
	i32 4127667938, ; 642: System.IO.FileSystem.Watcher => 0xf60736e2 => 50
	i32 4130442656, ; 643: System.AppContext => 0xf6318da0 => 6
	i32 4147896353, ; 644: System.Reflection.Emit.ILGeneration.dll => 0xf73be021 => 90
	i32 4150914736, ; 645: uk\Microsoft.Maui.Controls.resources => 0xf769eeb0 => 321
	i32 4151237749, ; 646: System.Core => 0xf76edc75 => 21
	i32 4159265925, ; 647: System.Xml.XmlSerializer => 0xf7e95c85 => 162
	i32 4161255271, ; 648: System.Reflection.TypeExtensions => 0xf807b767 => 96
	i32 4164802419, ; 649: System.IO.FileSystem.Watcher.dll => 0xf83dd773 => 50
	i32 4181436372, ; 650: System.Runtime.Serialization.Primitives => 0xf93ba7d4 => 113
	i32 4182413190, ; 651: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 250
	i32 4182880526, ; 652: Microsoft.VisualStudio.DesignTools.MobileTapContracts => 0xf951b10e => 328
	i32 4185676441, ; 653: System.Security => 0xf97c5a99 => 130
	i32 4196529839, ; 654: System.Net.WebClient.dll => 0xfa21f6af => 76
	i32 4213026141, ; 655: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 27
	i32 4256097574, ; 656: Xamarin.AndroidX.Core.Core.Ktx => 0xfdaee526 => 227
	i32 4258378803, ; 657: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx => 0xfdd1b433 => 249
	i32 4260525087, ; 658: System.Buffers => 0xfdf2741f => 7
	i32 4271975918, ; 659: Microsoft.Maui.Controls.dll => 0xfea12dee => 189
	i32 4274623895, ; 660: CommunityToolkit.Mvvm.dll => 0xfec99597 => 176
	i32 4274976490, ; 661: System.Runtime.Numerics => 0xfecef6ea => 110
	i32 4292120959, ; 662: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 250
	i32 4294763496 ; 663: Xamarin.AndroidX.ExifInterface.dll => 0xfffce3e8 => 236
], align 4

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [664 x i32] [
	i32 68, ; 0
	i32 67, ; 1
	i32 108, ; 2
	i32 246, ; 3
	i32 281, ; 4
	i32 48, ; 5
	i32 194, ; 6
	i32 80, ; 7
	i32 217, ; 8
	i32 145, ; 9
	i32 30, ; 10
	i32 325, ; 11
	i32 124, ; 12
	i32 193, ; 13
	i32 102, ; 14
	i32 264, ; 15
	i32 107, ; 16
	i32 264, ; 17
	i32 139, ; 18
	i32 285, ; 19
	i32 77, ; 20
	i32 124, ; 21
	i32 13, ; 22
	i32 220, ; 23
	i32 132, ; 24
	i32 266, ; 25
	i32 151, ; 26
	i32 322, ; 27
	i32 323, ; 28
	i32 18, ; 29
	i32 214, ; 30
	i32 26, ; 31
	i32 240, ; 32
	i32 1, ; 33
	i32 59, ; 34
	i32 42, ; 35
	i32 91, ; 36
	i32 223, ; 37
	i32 147, ; 38
	i32 242, ; 39
	i32 239, ; 40
	i32 294, ; 41
	i32 54, ; 42
	i32 69, ; 43
	i32 322, ; 44
	i32 205, ; 45
	i32 83, ; 46
	i32 307, ; 47
	i32 241, ; 48
	i32 198, ; 49
	i32 306, ; 50
	i32 131, ; 51
	i32 55, ; 52
	i32 149, ; 53
	i32 74, ; 54
	i32 145, ; 55
	i32 62, ; 56
	i32 146, ; 57
	i32 331, ; 58
	i32 165, ; 59
	i32 318, ; 60
	i32 224, ; 61
	i32 12, ; 62
	i32 0, ; 63
	i32 237, ; 64
	i32 125, ; 65
	i32 152, ; 66
	i32 113, ; 67
	i32 166, ; 68
	i32 164, ; 69
	i32 239, ; 70
	i32 252, ; 71
	i32 84, ; 72
	i32 305, ; 73
	i32 299, ; 74
	i32 187, ; 75
	i32 150, ; 76
	i32 285, ; 77
	i32 60, ; 78
	i32 183, ; 79
	i32 51, ; 80
	i32 103, ; 81
	i32 114, ; 82
	i32 40, ; 83
	i32 278, ; 84
	i32 275, ; 85
	i32 120, ; 86
	i32 313, ; 87
	i32 174, ; 88
	i32 52, ; 89
	i32 44, ; 90
	i32 119, ; 91
	i32 229, ; 92
	i32 311, ; 93
	i32 235, ; 94
	i32 81, ; 95
	i32 136, ; 96
	i32 271, ; 97
	i32 212, ; 98
	i32 8, ; 99
	i32 73, ; 100
	i32 293, ; 101
	i32 155, ; 102
	i32 287, ; 103
	i32 154, ; 104
	i32 92, ; 105
	i32 282, ; 106
	i32 45, ; 107
	i32 308, ; 108
	i32 296, ; 109
	i32 286, ; 110
	i32 109, ; 111
	i32 129, ; 112
	i32 196, ; 113
	i32 25, ; 114
	i32 202, ; 115
	i32 72, ; 116
	i32 55, ; 117
	i32 200, ; 118
	i32 46, ; 119
	i32 317, ; 120
	i32 277, ; 121
	i32 186, ; 122
	i32 230, ; 123
	i32 22, ; 124
	i32 244, ; 125
	i32 86, ; 126
	i32 43, ; 127
	i32 160, ; 128
	i32 71, ; 129
	i32 291, ; 130
	i32 257, ; 131
	i32 289, ; 132
	i32 3, ; 133
	i32 326, ; 134
	i32 42, ; 135
	i32 63, ; 136
	i32 16, ; 137
	i32 53, ; 138
	i32 320, ; 139
	i32 281, ; 140
	i32 105, ; 141
	i32 194, ; 142
	i32 286, ; 143
	i32 279, ; 144
	i32 241, ; 145
	i32 34, ; 146
	i32 158, ; 147
	i32 85, ; 148
	i32 32, ; 149
	i32 12, ; 150
	i32 51, ; 151
	i32 56, ; 152
	i32 261, ; 153
	i32 36, ; 154
	i32 182, ; 155
	i32 295, ; 156
	i32 280, ; 157
	i32 210, ; 158
	i32 35, ; 159
	i32 58, ; 160
	i32 248, ; 161
	i32 277, ; 162
	i32 178, ; 163
	i32 17, ; 164
	i32 283, ; 165
	i32 164, ; 166
	i32 308, ; 167
	i32 247, ; 168
	i32 185, ; 169
	i32 274, ; 170
	i32 314, ; 171
	i32 153, ; 172
	i32 270, ; 173
	i32 255, ; 174
	i32 312, ; 175
	i32 212, ; 176
	i32 29, ; 177
	i32 176, ; 178
	i32 52, ; 179
	i32 310, ; 180
	i32 275, ; 181
	i32 5, ; 182
	i32 294, ; 183
	i32 265, ; 184
	i32 269, ; 185
	i32 221, ; 186
	i32 287, ; 187
	i32 209, ; 188
	i32 197, ; 189
	i32 232, ; 190
	i32 85, ; 191
	i32 274, ; 192
	i32 61, ; 193
	i32 112, ; 194
	i32 57, ; 195
	i32 324, ; 196
	i32 261, ; 197
	i32 99, ; 198
	i32 19, ; 199
	i32 225, ; 200
	i32 111, ; 201
	i32 101, ; 202
	i32 102, ; 203
	i32 292, ; 204
	i32 104, ; 205
	i32 279, ; 206
	i32 71, ; 207
	i32 38, ; 208
	i32 32, ; 209
	i32 103, ; 210
	i32 73, ; 211
	i32 298, ; 212
	i32 9, ; 213
	i32 123, ; 214
	i32 46, ; 215
	i32 211, ; 216
	i32 187, ; 217
	i32 9, ; 218
	i32 43, ; 219
	i32 4, ; 220
	i32 262, ; 221
	i32 302, ; 222
	i32 297, ; 223
	i32 31, ; 224
	i32 138, ; 225
	i32 92, ; 226
	i32 93, ; 227
	i32 317, ; 228
	i32 49, ; 229
	i32 141, ; 230
	i32 112, ; 231
	i32 140, ; 232
	i32 231, ; 233
	i32 115, ; 234
	i32 280, ; 235
	i32 157, ; 236
	i32 327, ; 237
	i32 76, ; 238
	i32 79, ; 239
	i32 251, ; 240
	i32 37, ; 241
	i32 273, ; 242
	i32 175, ; 243
	i32 235, ; 244
	i32 228, ; 245
	i32 64, ; 246
	i32 138, ; 247
	i32 15, ; 248
	i32 116, ; 249
	i32 267, ; 250
	i32 276, ; 251
	i32 223, ; 252
	i32 48, ; 253
	i32 70, ; 254
	i32 80, ; 255
	i32 126, ; 256
	i32 94, ; 257
	i32 121, ; 258
	i32 284, ; 259
	i32 26, ; 260
	i32 198, ; 261
	i32 244, ; 262
	i32 97, ; 263
	i32 28, ; 264
	i32 219, ; 265
	i32 315, ; 266
	i32 293, ; 267
	i32 149, ; 268
	i32 169, ; 269
	i32 4, ; 270
	i32 98, ; 271
	i32 33, ; 272
	i32 93, ; 273
	i32 266, ; 274
	i32 183, ; 275
	i32 21, ; 276
	i32 41, ; 277
	i32 170, ; 278
	i32 309, ; 279
	i32 326, ; 280
	i32 237, ; 281
	i32 301, ; 282
	i32 251, ; 283
	i32 283, ; 284
	i32 276, ; 285
	i32 256, ; 286
	i32 2, ; 287
	i32 134, ; 288
	i32 111, ; 289
	i32 329, ; 290
	i32 184, ; 291
	i32 321, ; 292
	i32 202, ; 293
	i32 318, ; 294
	i32 58, ; 295
	i32 95, ; 296
	i32 300, ; 297
	i32 39, ; 298
	i32 213, ; 299
	i32 329, ; 300
	i32 25, ; 301
	i32 94, ; 302
	i32 89, ; 303
	i32 99, ; 304
	i32 10, ; 305
	i32 87, ; 306
	i32 100, ; 307
	i32 263, ; 308
	i32 179, ; 309
	i32 284, ; 310
	i32 204, ; 311
	i32 297, ; 312
	i32 7, ; 313
	i32 248, ; 314
	i32 292, ; 315
	i32 201, ; 316
	i32 88, ; 317
	i32 243, ; 318
	i32 154, ; 319
	i32 296, ; 320
	i32 33, ; 321
	i32 116, ; 322
	i32 82, ; 323
	i32 199, ; 324
	i32 330, ; 325
	i32 20, ; 326
	i32 0, ; 327
	i32 11, ; 328
	i32 162, ; 329
	i32 3, ; 330
	i32 191, ; 331
	i32 304, ; 332
	i32 186, ; 333
	i32 184, ; 334
	i32 84, ; 335
	i32 288, ; 336
	i32 64, ; 337
	i32 306, ; 338
	i32 270, ; 339
	i32 143, ; 340
	i32 252, ; 341
	i32 157, ; 342
	i32 41, ; 343
	i32 117, ; 344
	i32 180, ; 345
	i32 203, ; 346
	i32 300, ; 347
	i32 259, ; 348
	i32 131, ; 349
	i32 75, ; 350
	i32 66, ; 351
	i32 310, ; 352
	i32 172, ; 353
	i32 207, ; 354
	i32 143, ; 355
	i32 106, ; 356
	i32 151, ; 357
	i32 70, ; 358
	i32 156, ; 359
	i32 179, ; 360
	i32 121, ; 361
	i32 127, ; 362
	i32 305, ; 363
	i32 152, ; 364
	i32 234, ; 365
	i32 328, ; 366
	i32 141, ; 367
	i32 221, ; 368
	i32 302, ; 369
	i32 20, ; 370
	i32 14, ; 371
	i32 135, ; 372
	i32 75, ; 373
	i32 59, ; 374
	i32 196, ; 375
	i32 224, ; 376
	i32 167, ; 377
	i32 168, ; 378
	i32 189, ; 379
	i32 15, ; 380
	i32 74, ; 381
	i32 6, ; 382
	i32 23, ; 383
	i32 246, ; 384
	i32 201, ; 385
	i32 91, ; 386
	i32 303, ; 387
	i32 1, ; 388
	i32 173, ; 389
	i32 136, ; 390
	i32 247, ; 391
	i32 269, ; 392
	i32 134, ; 393
	i32 69, ; 394
	i32 146, ; 395
	i32 312, ; 396
	i32 288, ; 397
	i32 238, ; 398
	i32 185, ; 399
	i32 88, ; 400
	i32 96, ; 401
	i32 228, ; 402
	i32 233, ; 403
	i32 307, ; 404
	i32 31, ; 405
	i32 45, ; 406
	i32 242, ; 407
	i32 203, ; 408
	i32 109, ; 409
	i32 158, ; 410
	i32 35, ; 411
	i32 22, ; 412
	i32 114, ; 413
	i32 57, ; 414
	i32 267, ; 415
	i32 144, ; 416
	i32 118, ; 417
	i32 120, ; 418
	i32 110, ; 419
	i32 205, ; 420
	i32 139, ; 421
	i32 211, ; 422
	i32 54, ; 423
	i32 105, ; 424
	i32 313, ; 425
	i32 190, ; 426
	i32 191, ; 427
	i32 133, ; 428
	i32 282, ; 429
	i32 272, ; 430
	i32 260, ; 431
	i32 319, ; 432
	i32 238, ; 433
	i32 193, ; 434
	i32 159, ; 435
	i32 298, ; 436
	i32 225, ; 437
	i32 163, ; 438
	i32 132, ; 439
	i32 260, ; 440
	i32 161, ; 441
	i32 311, ; 442
	i32 177, ; 443
	i32 249, ; 444
	i32 140, ; 445
	i32 272, ; 446
	i32 268, ; 447
	i32 169, ; 448
	i32 192, ; 449
	i32 175, ; 450
	i32 206, ; 451
	i32 278, ; 452
	i32 40, ; 453
	i32 236, ; 454
	i32 81, ; 455
	i32 56, ; 456
	i32 37, ; 457
	i32 97, ; 458
	i32 166, ; 459
	i32 172, ; 460
	i32 273, ; 461
	i32 82, ; 462
	i32 208, ; 463
	i32 98, ; 464
	i32 30, ; 465
	i32 159, ; 466
	i32 18, ; 467
	i32 218, ; 468
	i32 127, ; 469
	i32 119, ; 470
	i32 232, ; 471
	i32 263, ; 472
	i32 216, ; 473
	i32 245, ; 474
	i32 218, ; 475
	i32 265, ; 476
	i32 165, ; 477
	i32 240, ; 478
	i32 331, ; 479
	i32 216, ; 480
	i32 262, ; 481
	i32 253, ; 482
	i32 170, ; 483
	i32 16, ; 484
	i32 144, ; 485
	i32 304, ; 486
	i32 125, ; 487
	i32 118, ; 488
	i32 38, ; 489
	i32 115, ; 490
	i32 47, ; 491
	i32 142, ; 492
	i32 117, ; 493
	i32 34, ; 494
	i32 178, ; 495
	i32 95, ; 496
	i32 53, ; 497
	i32 254, ; 498
	i32 129, ; 499
	i32 153, ; 500
	i32 24, ; 501
	i32 161, ; 502
	i32 231, ; 503
	i32 289, ; 504
	i32 330, ; 505
	i32 148, ; 506
	i32 104, ; 507
	i32 89, ; 508
	i32 219, ; 509
	i32 60, ; 510
	i32 142, ; 511
	i32 100, ; 512
	i32 5, ; 513
	i32 13, ; 514
	i32 290, ; 515
	i32 195, ; 516
	i32 122, ; 517
	i32 177, ; 518
	i32 135, ; 519
	i32 28, ; 520
	i32 299, ; 521
	i32 72, ; 522
	i32 229, ; 523
	i32 24, ; 524
	i32 213, ; 525
	i32 258, ; 526
	i32 255, ; 527
	i32 316, ; 528
	i32 137, ; 529
	i32 197, ; 530
	i32 206, ; 531
	i32 226, ; 532
	i32 168, ; 533
	i32 259, ; 534
	i32 295, ; 535
	i32 101, ; 536
	i32 123, ; 537
	i32 230, ; 538
	i32 215, ; 539
	i32 215, ; 540
	i32 181, ; 541
	i32 163, ; 542
	i32 167, ; 543
	i32 233, ; 544
	i32 173, ; 545
	i32 39, ; 546
	i32 188, ; 547
	i32 303, ; 548
	i32 17, ; 549
	i32 171, ; 550
	i32 316, ; 551
	i32 315, ; 552
	i32 137, ; 553
	i32 150, ; 554
	i32 222, ; 555
	i32 155, ; 556
	i32 130, ; 557
	i32 19, ; 558
	i32 65, ; 559
	i32 147, ; 560
	i32 47, ; 561
	i32 323, ; 562
	i32 204, ; 563
	i32 79, ; 564
	i32 61, ; 565
	i32 106, ; 566
	i32 257, ; 567
	i32 208, ; 568
	i32 49, ; 569
	i32 243, ; 570
	i32 320, ; 571
	i32 254, ; 572
	i32 14, ; 573
	i32 180, ; 574
	i32 68, ; 575
	i32 171, ; 576
	i32 217, ; 577
	i32 327, ; 578
	i32 214, ; 579
	i32 222, ; 580
	i32 325, ; 581
	i32 78, ; 582
	i32 227, ; 583
	i32 108, ; 584
	i32 207, ; 585
	i32 253, ; 586
	i32 67, ; 587
	i32 63, ; 588
	i32 27, ; 589
	i32 160, ; 590
	i32 291, ; 591
	i32 199, ; 592
	i32 220, ; 593
	i32 10, ; 594
	i32 188, ; 595
	i32 11, ; 596
	i32 174, ; 597
	i32 78, ; 598
	i32 126, ; 599
	i32 83, ; 600
	i32 200, ; 601
	i32 182, ; 602
	i32 290, ; 603
	i32 66, ; 604
	i32 107, ; 605
	i32 65, ; 606
	i32 128, ; 607
	i32 122, ; 608
	i32 195, ; 609
	i32 77, ; 610
	i32 268, ; 611
	i32 258, ; 612
	i32 324, ; 613
	i32 8, ; 614
	i32 226, ; 615
	i32 2, ; 616
	i32 44, ; 617
	i32 271, ; 618
	i32 156, ; 619
	i32 128, ; 620
	i32 256, ; 621
	i32 23, ; 622
	i32 133, ; 623
	i32 210, ; 624
	i32 245, ; 625
	i32 319, ; 626
	i32 301, ; 627
	i32 29, ; 628
	i32 209, ; 629
	i32 62, ; 630
	i32 190, ; 631
	i32 90, ; 632
	i32 87, ; 633
	i32 148, ; 634
	i32 192, ; 635
	i32 36, ; 636
	i32 86, ; 637
	i32 234, ; 638
	i32 314, ; 639
	i32 309, ; 640
	i32 181, ; 641
	i32 50, ; 642
	i32 6, ; 643
	i32 90, ; 644
	i32 321, ; 645
	i32 21, ; 646
	i32 162, ; 647
	i32 96, ; 648
	i32 50, ; 649
	i32 113, ; 650
	i32 250, ; 651
	i32 328, ; 652
	i32 130, ; 653
	i32 76, ; 654
	i32 27, ; 655
	i32 227, ; 656
	i32 249, ; 657
	i32 7, ; 658
	i32 189, ; 659
	i32 176, ; 660
	i32 110, ; 661
	i32 250, ; 662
	i32 236 ; 663
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
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }

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
!7 = !{i32 1, !"min_enum_size", i32 4}
