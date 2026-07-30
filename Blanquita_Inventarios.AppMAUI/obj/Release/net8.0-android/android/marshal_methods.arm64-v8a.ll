; ModuleID = 'marshal_methods.arm64-v8a.ll'
source_filename = "marshal_methods.arm64-v8a.ll"
target datalayout = "e-m:e-i8:8:32-i16:16:32-i64:64-i128:128-n32:64-S128"
target triple = "aarch64-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [166 x ptr] zeroinitializer, align 8

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [332 x i64] [
	i64 98382396393917666, ; 0: Microsoft.Extensions.Primitives.dll => 0x15d8644ad360ce2 => 47
	i64 120698629574877762, ; 1: Mono.Android => 0x1accec39cafe242 => 165
	i64 131669012237370309, ; 2: Microsoft.Maui.Essentials.dll => 0x1d3c844de55c3c5 => 52
	i64 196720943101637631, ; 3: System.Linq.Expressions.dll => 0x2bae4a7cd73f3ff => 121
	i64 210515253464952879, ; 4: Xamarin.AndroidX.Collection.dll => 0x2ebe681f694702f => 68
	i64 232391251801502327, ; 5: Xamarin.AndroidX.SavedState.dll => 0x3399e9cbc897277 => 85
	i64 545109961164950392, ; 6: fi/Microsoft.Maui.Controls.resources.dll => 0x7909e9f1ec38b78 => 7
	i64 560278790331054453, ; 7: System.Reflection.Primitives => 0x7c6829760de3975 => 139
	i64 608964066279188065, ; 8: Blanquita_Inventarios.Entities => 0x8737997838c6661 => 96
	i64 750875890346172408, ; 9: System.Threading.Thread => 0xa6ba5a4da7d1ff8 => 151
	i64 799765834175365804, ; 10: System.ComponentModel.dll => 0xb1956c9f18442ac => 107
	i64 849051935479314978, ; 11: hi/Microsoft.Maui.Controls.resources.dll => 0xbc8703ca21a3a22 => 10
	i64 870603111519317375, ; 12: SQLitePCLRaw.lib.e_sqlite3.android => 0xc1500ead2756d7f => 58
	i64 872800313462103108, ; 13: Xamarin.AndroidX.DrawerLayout => 0xc1ccf42c3c21c44 => 73
	i64 1010599046655515943, ; 14: System.Reflection.Primitives.dll => 0xe065e7a82401d27 => 139
	i64 1120440138749646132, ; 15: Xamarin.Google.Android.Material.dll => 0xf8c9a5eae431534 => 89
	i64 1121665720830085036, ; 16: nb/Microsoft.Maui.Controls.resources.dll => 0xf90f507becf47ac => 18
	i64 1268860745194512059, ; 17: System.Drawing.dll => 0x119be62002c19ebb => 116
	i64 1301485588176585670, ; 18: SQLitePCLRaw.core => 0x120fce3f338e43c6 => 57
	i64 1369545283391376210, ; 19: Xamarin.AndroidX.Navigation.Fragment.dll => 0x13019a2dd85acb52 => 81
	i64 1476839205573959279, ; 20: System.Net.Primitives.dll => 0x147ec96ece9b1e6f => 128
	i64 1486715745332614827, ; 21: Microsoft.Maui.Controls.dll => 0x14a1e017ea87d6ab => 49
	i64 1513467482682125403, ; 22: Mono.Android.Runtime => 0x1500eaa8245f6c5b => 164
	i64 1518315023656898250, ; 23: SQLitePCLRaw.provider.e_sqlite3 => 0x151223783a354eca => 59
	i64 1537168428375924959, ; 24: System.Threading.Thread.dll => 0x15551e8a954ae0df => 151
	i64 1556147632182429976, ; 25: ko/Microsoft.Maui.Controls.resources.dll => 0x15988c06d24c8918 => 16
	i64 1624659445732251991, ; 26: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0x168bf32877da9957 => 63
	i64 1628611045998245443, ; 27: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0x1699fd1e1a00b643 => 78
	i64 1731380447121279447, ; 28: Newtonsoft.Json => 0x18071957e9b889d7 => 54
	i64 1735388228521408345, ; 29: System.Net.Mail.dll => 0x181556663c69b759 => 126
	i64 1743969030606105336, ; 30: System.Memory.dll => 0x1833d297e88f2af8 => 124
	i64 1767386781656293639, ; 31: System.Private.Uri.dll => 0x188704e9f5582107 => 134
	i64 1795316252682057001, ; 32: Xamarin.AndroidX.AppCompat.dll => 0x18ea3e9eac997529 => 62
	i64 1825687700144851180, ; 33: System.Runtime.InteropServices.RuntimeInformation.dll => 0x1956254a55ef08ec => 140
	i64 1835311033149317475, ; 34: es\Microsoft.Maui.Controls.resources => 0x197855a927386163 => 6
	i64 1836611346387731153, ; 35: Xamarin.AndroidX.SavedState => 0x197cf449ebe482d1 => 85
	i64 1875417405349196092, ; 36: System.Drawing.Primitives => 0x1a06d2319b6c713c => 115
	i64 1881198190668717030, ; 37: tr\Microsoft.Maui.Controls.resources => 0x1a1b5bc992ea9be6 => 28
	i64 1920760634179481754, ; 38: Microsoft.Maui.Controls.Xaml => 0x1aa7e99ec2d2709a => 50
	i64 1930726298510463061, ; 39: CommunityToolkit.Mvvm.dll => 0x1acb5156cd389055 => 38
	i64 1959996714666907089, ; 40: tr/Microsoft.Maui.Controls.resources.dll => 0x1b334ea0a2a755d1 => 28
	i64 1981742497975770890, ; 41: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x1b80904d5c241f0a => 77
	i64 1983698669889758782, ; 42: cs/Microsoft.Maui.Controls.resources.dll => 0x1b87836e2031a63e => 2
	i64 2019660174692588140, ; 43: pl/Microsoft.Maui.Controls.resources.dll => 0x1c07463a6f8e1a6c => 20
	i64 2102659300918482391, ; 44: System.Drawing.Primitives.dll => 0x1d2e257e6aead5d7 => 115
	i64 2133195048986300728, ; 45: Newtonsoft.Json.dll => 0x1d9aa1984b735138 => 54
	i64 2262844636196693701, ; 46: Xamarin.AndroidX.DrawerLayout.dll => 0x1f673d352266e6c5 => 73
	i64 2287834202362508563, ; 47: System.Collections.Concurrent => 0x1fc00515e8ce7513 => 100
	i64 2302323944321350744, ; 48: ru/Microsoft.Maui.Controls.resources.dll => 0x1ff37f6ddb267c58 => 24
	i64 2329709569556905518, ; 49: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x2054ca829b447e2e => 76
	i64 2470498323731680442, ; 50: Xamarin.AndroidX.CoordinatorLayout => 0x2248f922dc398cba => 69
	i64 2497223385847772520, ; 51: System.Runtime => 0x22a7eb7046413568 => 146
	i64 2547086958574651984, ; 52: Xamarin.AndroidX.Activity.dll => 0x2359121801df4a50 => 61
	i64 2602673633151553063, ; 53: th\Microsoft.Maui.Controls.resources => 0x241e8de13a460e27 => 27
	i64 2624866290265602282, ; 54: mscorlib.dll => 0x246d65fbde2db8ea => 160
	i64 2632269733008246987, ; 55: System.Net.NameResolution => 0x2487b36034f808cb => 127
	i64 2656907746661064104, ; 56: Microsoft.Extensions.DependencyInjection => 0x24df3b84c8b75da8 => 42
	i64 2662981627730767622, ; 57: cs\Microsoft.Maui.Controls.resources => 0x24f4cfae6c48af06 => 2
	i64 2895129759130297543, ; 58: fi\Microsoft.Maui.Controls.resources => 0x282d912d479fa4c7 => 7
	i64 3017704767998173186, ; 59: Xamarin.Google.Android.Material => 0x29e10a7f7d88a002 => 89
	i64 3289520064315143713, ; 60: Xamarin.AndroidX.Lifecycle.Common => 0x2da6b911e3063621 => 75
	i64 3311221304742556517, ; 61: System.Numerics.Vectors.dll => 0x2df3d23ba9e2b365 => 132
	i64 3325875462027654285, ; 62: System.Runtime.Numerics => 0x2e27e21c8958b48d => 143
	i64 3328853167529574890, ; 63: System.Net.Sockets.dll => 0x2e327651a008c1ea => 131
	i64 3344514922410554693, ; 64: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x2e6a1a9a18463545 => 92
	i64 3429672777697402584, ; 65: Microsoft.Maui.Essentials => 0x2f98a5385a7b1ed8 => 52
	i64 3494946837667399002, ; 66: Microsoft.Extensions.Configuration => 0x30808ba1c00a455a => 40
	i64 3522470458906976663, ; 67: Xamarin.AndroidX.SwipeRefreshLayout => 0x30e2543832f52197 => 86
	i64 3551103847008531295, ; 68: System.Private.CoreLib.dll => 0x31480e226177735f => 162
	i64 3567343442040498961, ; 69: pt\Microsoft.Maui.Controls.resources => 0x3181bff5bea4ab11 => 22
	i64 3571415421602489686, ; 70: System.Runtime.dll => 0x319037675df7e556 => 146
	i64 3638003163729360188, ; 71: Microsoft.Extensions.Configuration.Abstractions => 0x327cc89a39d5f53c => 41
	i64 3647754201059316852, ; 72: System.Xml.ReaderWriter => 0x329f6d1e86145474 => 157
	i64 3655542548057982301, ; 73: Microsoft.Extensions.Configuration.dll => 0x32bb18945e52855d => 40
	i64 3716579019761409177, ; 74: netstandard.dll => 0x3393f0ed5c8c5c99 => 161
	i64 3727469159507183293, ; 75: Xamarin.AndroidX.RecyclerView => 0x33baa1739ba646bd => 84
	i64 3869221888984012293, ; 76: Microsoft.Extensions.Logging.dll => 0x35b23cceda0ed605 => 44
	i64 3890352374528606784, ; 77: Microsoft.Maui.Controls.Xaml.dll => 0x35fd4edf66e00240 => 50
	i64 3933965368022646939, ; 78: System.Net.Requests => 0x369840a8bfadc09b => 129
	i64 3966267475168208030, ; 79: System.Memory => 0x370b03412596249e => 124
	i64 4009997192427317104, ; 80: System.Runtime.Serialization.Primitives => 0x37a65f335cf1a770 => 145
	i64 4073500526318903918, ; 81: System.Private.Xml.dll => 0x3887fb25779ae26e => 136
	i64 4073631083018132676, ; 82: Microsoft.Maui.Controls.Compatibility.dll => 0x388871e311491cc4 => 48
	i64 4120493066591692148, ; 83: zh-Hant\Microsoft.Maui.Controls.resources => 0x392eee9cdda86574 => 33
	i64 4154383907710350974, ; 84: System.ComponentModel => 0x39a7562737acb67e => 107
	i64 4167269041631776580, ; 85: System.Threading.ThreadPool => 0x39d51d1d3df1cf44 => 152
	i64 4187479170553454871, ; 86: System.Linq.Expressions => 0x3a1cea1e912fa117 => 121
	i64 4205801962323029395, ; 87: System.ComponentModel.TypeConverter => 0x3a5e0299f7e7ad93 => 106
	i64 4337444564132831293, ; 88: SQLitePCLRaw.batteries_v2.dll => 0x3c31b2d9ae16203d => 56
	i64 4356591372459378815, ; 89: vi/Microsoft.Maui.Controls.resources.dll => 0x3c75b8c562f9087f => 30
	i64 4373617458794931033, ; 90: System.IO.Pipes.dll => 0x3cb235e806eb2359 => 120
	i64 4477672992252076438, ; 91: System.Web.HttpUtility.dll => 0x3e23e3dcdb8ba196 => 155
	i64 4620536241703500132, ; 92: System.Data.SqlClient.dll => 0x401f713b2e8e3964 => 60
	i64 4679594760078841447, ; 93: ar/Microsoft.Maui.Controls.resources.dll => 0x40f142a407475667 => 0
	i64 4725285941539738176, ; 94: Xamarin.AndroidX.Camera.Lifecycle => 0x41939687379f9240 => 65
	i64 4794310189461587505, ; 95: Xamarin.AndroidX.Activity => 0x4288cfb749e4c631 => 61
	i64 4795410492532947900, ; 96: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0x428cb86f8f9b7bbc => 86
	i64 4809057822547766521, ; 97: System.Drawing => 0x42bd349c3145ecf9 => 116
	i64 4814660307502931973, ; 98: System.Net.NameResolution.dll => 0x42d11c0a5ee2a005 => 127
	i64 4853321196694829351, ; 99: System.Runtime.Loader.dll => 0x435a75ea15de7927 => 142
	i64 5103417709280584325, ; 100: System.Collections.Specialized => 0x46d2fb5e161b6285 => 103
	i64 5182934613077526976, ; 101: System.Collections.Specialized.dll => 0x47ed7b91fa9009c0 => 103
	i64 5266851800019912408, ; 102: System.Data.SqlClient => 0x49179dcea0d046d8 => 60
	i64 5290786973231294105, ; 103: System.Runtime.Loader => 0x496ca6b869b72699 => 142
	i64 5423376490970181369, ; 104: System.Runtime.InteropServices.RuntimeInformation => 0x4b43b42f2b7b6ef9 => 140
	i64 5471532531798518949, ; 105: sv\Microsoft.Maui.Controls.resources => 0x4beec9d926d82ca5 => 26
	i64 5507995362134886206, ; 106: System.Core.dll => 0x4c705499688c873e => 109
	i64 5522859530602327440, ; 107: uk\Microsoft.Maui.Controls.resources => 0x4ca5237b51eead90 => 29
	i64 5570799893513421663, ; 108: System.IO.Compression.Brotli => 0x4d4f74fcdfa6c35f => 118
	i64 5573260873512690141, ; 109: System.Security.Cryptography.dll => 0x4d58333c6e4ea1dd => 147
	i64 5692067934154308417, ; 110: Xamarin.AndroidX.ViewPager2.dll => 0x4efe49a0d4a8bb41 => 88
	i64 5979151488806146654, ; 111: System.Formats.Asn1 => 0x52fa3699a489d25e => 117
	i64 6068057819846744445, ; 112: ro/Microsoft.Maui.Controls.resources.dll => 0x5436126fec7f197d => 23
	i64 6183170893902868313, ; 113: SQLitePCLRaw.batteries_v2 => 0x55cf092b0c9d6f59 => 56
	i64 6200764641006662125, ; 114: ro\Microsoft.Maui.Controls.resources => 0x560d8a96830131ed => 23
	i64 6251069312384999852, ; 115: System.Transactions.Local => 0x56c0426b870da1ac => 154
	i64 6284145129771520194, ; 116: System.Reflection.Emit.ILGeneration => 0x5735c4b3610850c2 => 137
	i64 6357457916754632952, ; 117: _Microsoft.Android.Resource.Designer => 0x583a3a4ac2a7a0f8 => 34
	i64 6401687960814735282, ; 118: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0x58d75d486341cfb2 => 76
	i64 6478287442656530074, ; 119: hr\Microsoft.Maui.Controls.resources => 0x59e7801b0c6a8e9a => 11
	i64 6548213210057960872, ; 120: Xamarin.AndroidX.CustomView.dll => 0x5adfed387b066da8 => 72
	i64 6560151584539558821, ; 121: Microsoft.Extensions.Options => 0x5b0a571be53243a5 => 46
	i64 6617685658146568858, ; 122: System.Text.Encoding.CodePages => 0x5bd6be0b4905fa9a => 148
	i64 6743165466166707109, ; 123: nl\Microsoft.Maui.Controls.resources => 0x5d948943c08c43a5 => 19
	i64 6777482997383978746, ; 124: pt/Microsoft.Maui.Controls.resources.dll => 0x5e0e74e0a2525efa => 22
	i64 6786606130239981554, ; 125: System.Diagnostics.TraceSource => 0x5e2ede51877147f2 => 113
	i64 6814185388980153342, ; 126: System.Xml.XDocument.dll => 0x5e90d98217d1abfe => 158
	i64 6876862101832370452, ; 127: System.Xml.Linq => 0x5f6f85a57d108914 => 156
	i64 6894844156784520562, ; 128: System.Numerics.Vectors => 0x5faf683aead1ad72 => 132
	i64 6985504089449394141, ; 129: ZXing.Net.MAUI.Controls.dll => 0x60f17ef564ab6fdd => 95
	i64 7083547580668757502, ; 130: System.Private.Xml.Linq.dll => 0x624dd0fe8f56c5fe => 135
	i64 7141972834282579846, ; 131: Blanquita_Inventarios.AppMAUI => 0x631d6273f6e72786 => 97
	i64 7156322743841701417, ; 132: Blanquita_Inventarios.AppMAUI.dll => 0x63505d9ea4bbbe29 => 97
	i64 7220009545223068405, ; 133: sv/Microsoft.Maui.Controls.resources.dll => 0x6432a06d99f35af5 => 26
	i64 7270811800166795866, ; 134: System.Linq => 0x64e71ccf51a90a5a => 123
	i64 7316205155833392065, ; 135: Microsoft.Win32.Primitives => 0x658861d38954abc1 => 99
	i64 7377312882064240630, ; 136: System.ComponentModel.TypeConverter.dll => 0x66617afac45a2ff6 => 106
	i64 7488575175965059935, ; 137: System.Xml.Linq.dll => 0x67ecc3724534ab5f => 156
	i64 7489048572193775167, ; 138: System.ObjectModel => 0x67ee71ff6b419e3f => 133
	i64 7592577537120840276, ; 139: System.Diagnostics.Process => 0x695e410af5b2aa54 => 112
	i64 7654504624184590948, ; 140: System.Net.Http => 0x6a3a4366801b8264 => 125
	i64 7694700312542370399, ; 141: System.Net.Mail => 0x6ac9112a7e2cda5f => 126
	i64 7708790323521193081, ; 142: ms/Microsoft.Maui.Controls.resources.dll => 0x6afb1ff4d1730479 => 17
	i64 7714652370974252055, ; 143: System.Private.CoreLib => 0x6b0ff375198b9c17 => 162
	i64 7735176074855944702, ; 144: Microsoft.CSharp => 0x6b58dda848e391fe => 98
	i64 7735352534559001595, ; 145: Xamarin.Kotlin.StdLib.dll => 0x6b597e2582ce8bfb => 91
	i64 7836164640616011524, ; 146: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x6cbfa6390d64d704 => 63
	i64 7875371864198757046, ; 147: AndHUD.dll => 0x6d4af0fc27ac3ab6 => 35
	i64 8064050204834738623, ; 148: System.Collections.dll => 0x6fe942efa61731bf => 104
	i64 8083354569033831015, ; 149: Xamarin.AndroidX.Lifecycle.Common.dll => 0x702dd82730cad267 => 75
	i64 8087206902342787202, ; 150: System.Diagnostics.DiagnosticSource => 0x703b87d46f3aa082 => 111
	i64 8167236081217502503, ; 151: Java.Interop.dll => 0x7157d9f1a9b8fd27 => 163
	i64 8185542183669246576, ; 152: System.Collections => 0x7198e33f4794aa70 => 104
	i64 8246048515196606205, ; 153: Microsoft.Maui.Graphics.dll => 0x726fd96f64ee56fd => 53
	i64 8264926008854159966, ; 154: System.Diagnostics.Process.dll => 0x72b2ea6a64a3a25e => 112
	i64 8320777595162576093, ; 155: Xamarin.AndroidX.Camera.View => 0x737957232eb1c4dd => 66
	i64 8368701292315763008, ; 156: System.Security.Cryptography => 0x7423997c6fd56140 => 147
	i64 8400357532724379117, ; 157: Xamarin.AndroidX.Navigation.UI.dll => 0x749410ab44503ded => 83
	i64 8410671156615598628, ; 158: System.Reflection.Emit.Lightweight.dll => 0x74b8b4daf4b25224 => 138
	i64 8518412311883997971, ; 159: System.Collections.Immutable => 0x76377add7c28e313 => 101
	i64 8563666267364444763, ; 160: System.Private.Uri => 0x76d841191140ca5b => 134
	i64 8599632406834268464, ; 161: CommunityToolkit.Maui => 0x7758081c784b4930 => 36
	i64 8614108721271900878, ; 162: pt-BR/Microsoft.Maui.Controls.resources.dll => 0x778b763e14018ace => 21
	i64 8626175481042262068, ; 163: Java.Interop => 0x77b654e585b55834 => 163
	i64 8629545377263870989, ; 164: Xamarin.AndroidX.Camera.Core.dll => 0x77c24dcca0ed640d => 64
	i64 8638972117149407195, ; 165: Microsoft.CSharp.dll => 0x77e3cb5e8b31d7db => 98
	i64 8639588376636138208, ; 166: Xamarin.AndroidX.Navigation.Runtime => 0x77e5fbdaa2fda2e0 => 82
	i64 8677882282824630478, ; 167: pt-BR\Microsoft.Maui.Controls.resources => 0x786e07f5766b00ce => 21
	i64 8725526185868997716, ; 168: System.Diagnostics.DiagnosticSource.dll => 0x79174bd613173454 => 111
	i64 8941376889969657626, ; 169: System.Xml.XDocument => 0x7c1626e87187471a => 158
	i64 9045785047181495996, ; 170: zh-HK\Microsoft.Maui.Controls.resources => 0x7d891592e3cb0ebc => 31
	i64 9131857290992441898, ; 171: Xamarin.AndroidX.Camera.Core => 0x7ebadfd2d12a5a2a => 64
	i64 9312692141327339315, ; 172: Xamarin.AndroidX.ViewPager2 => 0x813d54296a634f33 => 88
	i64 9324707631942237306, ; 173: Xamarin.AndroidX.AppCompat => 0x8168042fd44a7c7a => 62
	i64 9659729154652888475, ; 174: System.Text.RegularExpressions => 0x860e407c9991dd9b => 150
	i64 9678050649315576968, ; 175: Xamarin.AndroidX.CoordinatorLayout.dll => 0x864f57c9feb18c88 => 69
	i64 9702891218465930390, ; 176: System.Collections.NonGeneric.dll => 0x86a79827b2eb3c96 => 102
	i64 9780723996889434231, ; 177: AndHUD => 0x87bc1ca798bbc877 => 35
	i64 9808709177481450983, ; 178: Mono.Android.dll => 0x881f890734e555e7 => 165
	i64 9933555792566666578, ; 179: System.Linq.Queryable.dll => 0x89db145cf475c552 => 122
	i64 9956195530459977388, ; 180: Microsoft.Maui => 0x8a2b8315b36616ac => 51
	i64 9991543690424095600, ; 181: es/Microsoft.Maui.Controls.resources.dll => 0x8aa9180c89861370 => 6
	i64 10038780035334861115, ; 182: System.Net.Http.dll => 0x8b50e941206af13b => 125
	i64 10051358222726253779, ; 183: System.Private.Xml => 0x8b7d990c97ccccd3 => 136
	i64 10092835686693276772, ; 184: Microsoft.Maui.Controls => 0x8c10f49539bd0c64 => 49
	i64 10143853363526200146, ; 185: da\Microsoft.Maui.Controls.resources => 0x8cc634e3c2a16b52 => 3
	i64 10229024438826829339, ; 186: Xamarin.AndroidX.CustomView => 0x8df4cb880b10061b => 72
	i64 10245369515835430794, ; 187: System.Reflection.Emit.Lightweight => 0x8e2edd4ad7fc978a => 138
	i64 10364469296367737616, ; 188: System.Reflection.Emit.ILGeneration.dll => 0x8fd5fde967711b10 => 137
	i64 10406448008575299332, ; 189: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x906b2153fcb3af04 => 92
	i64 10430153318873392755, ; 190: Xamarin.AndroidX.Core => 0x90bf592ea44f6673 => 70
	i64 10506226065143327199, ; 191: ca\Microsoft.Maui.Controls.resources => 0x91cd9cf11ed169df => 1
	i64 10670374202010151210, ; 192: Microsoft.Win32.Primitives.dll => 0x9414c8cd7b4ea92a => 99
	i64 10785150219063592792, ; 193: System.Net.Primitives => 0x95ac8cfb68830758 => 128
	i64 10822644899632537592, ; 194: System.Linq.Queryable => 0x9631c23204ca5ff8 => 122
	i64 10880838204485145808, ; 195: CommunityToolkit.Maui.dll => 0x970080b2a4d614d0 => 36
	i64 10964653383833615866, ; 196: System.Diagnostics.Tracing => 0x982a4628ccaffdfa => 114
	i64 11002576679268595294, ; 197: Microsoft.Extensions.Logging.Abstractions => 0x98b1013215cd365e => 45
	i64 11009005086950030778, ; 198: Microsoft.Maui.dll => 0x98c7d7cc621ffdba => 51
	i64 11023048688141570732, ; 199: System.Core => 0x98f9bc61168392ac => 109
	i64 11103970607964515343, ; 200: hu\Microsoft.Maui.Controls.resources => 0x9a193a6fc41a6c0f => 12
	i64 11162124722117608902, ; 201: Xamarin.AndroidX.ViewPager => 0x9ae7d54b986d05c6 => 87
	i64 11220793807500858938, ; 202: ja\Microsoft.Maui.Controls.resources => 0x9bb8448481fdd63a => 15
	i64 11226290749488709958, ; 203: Microsoft.Extensions.Options.dll => 0x9bcbcbf50c874146 => 46
	i64 11340910727871153756, ; 204: Xamarin.AndroidX.CursorAdapter => 0x9d630238642d465c => 71
	i64 11485890710487134646, ; 205: System.Runtime.InteropServices => 0x9f6614bf0f8b71b6 => 141
	i64 11518296021396496455, ; 206: id\Microsoft.Maui.Controls.resources => 0x9fd9353475222047 => 13
	i64 11529969570048099689, ; 207: Xamarin.AndroidX.ViewPager.dll => 0xa002ae3c4dc7c569 => 87
	i64 11530571088791430846, ; 208: Microsoft.Extensions.Logging => 0xa004d1504ccd66be => 44
	i64 11597940890313164233, ; 209: netstandard => 0xa0f429ca8d1805c9 => 161
	i64 11705530742807338875, ; 210: he/Microsoft.Maui.Controls.resources.dll => 0xa272663128721f7b => 9
	i64 11739066727115742305, ; 211: SQLite-net.dll => 0xa2e98afdf8575c61 => 55
	i64 11806260347154423189, ; 212: SQLite-net => 0xa3d8433bc5eb5d95 => 55
	i64 12201331334810686224, ; 213: System.Runtime.Serialization.Primitives.dll => 0xa953d6341e3bd310 => 145
	i64 12269460666702402136, ; 214: System.Collections.Immutable.dll => 0xaa45e178506c9258 => 101
	i64 12279246230491828964, ; 215: SQLitePCLRaw.provider.e_sqlite3.dll => 0xaa68a5636e0512e4 => 59
	i64 12341818387765915815, ; 216: CommunityToolkit.Maui.Core.dll => 0xab46f26f152bf0a7 => 37
	i64 12451044538927396471, ; 217: Xamarin.AndroidX.Fragment.dll => 0xaccaff0a2955b677 => 74
	i64 12466513435562512481, ; 218: Xamarin.AndroidX.Loader.dll => 0xad01f3eb52569061 => 79
	i64 12475113361194491050, ; 219: _Microsoft.Android.Resource.Designer.dll => 0xad2081818aba1caa => 34
	i64 12517810545449516888, ; 220: System.Diagnostics.TraceSource.dll => 0xadb8325e6f283f58 => 113
	i64 12538491095302438457, ; 221: Xamarin.AndroidX.CardView.dll => 0xae01ab382ae67e39 => 67
	i64 12550732019250633519, ; 222: System.IO.Compression => 0xae2d28465e8e1b2f => 119
	i64 12681088699309157496, ; 223: it/Microsoft.Maui.Controls.resources.dll => 0xaffc46fc178aec78 => 14
	i64 12700543734426720211, ; 224: Xamarin.AndroidX.Collection => 0xb041653c70d157d3 => 68
	i64 12708922737231849740, ; 225: System.Text.Encoding.Extensions => 0xb05f29e50e96e90c => 149
	i64 12748349129307211441, ; 226: Controls.UserDialogs.Maui => 0xb0eb3bfd4d0446b1 => 39
	i64 12760970142902902754, ; 227: Xamarin.AndroidX.Camera.Lifecycle.dll => 0xb11812bc0517dfe2 => 65
	i64 12823819093633476069, ; 228: th/Microsoft.Maui.Controls.resources.dll => 0xb1f75b85abe525e5 => 27
	i64 12835242264250840079, ; 229: System.IO.Pipes => 0xb21ff0d5d6c0740f => 120
	i64 12843321153144804894, ; 230: Microsoft.Extensions.Primitives => 0xb23ca48abd74d61e => 47
	i64 13068258254871114833, ; 231: System.Runtime.Serialization.Formatters.dll => 0xb55bc7a4eaa8b451 => 144
	i64 13221551921002590604, ; 232: ca/Microsoft.Maui.Controls.resources.dll => 0xb77c636bdebe318c => 1
	i64 13222659110913276082, ; 233: ja/Microsoft.Maui.Controls.resources.dll => 0xb78052679c1178b2 => 15
	i64 13343850469010654401, ; 234: Mono.Android.Runtime.dll => 0xb92ee14d854f44c1 => 164
	i64 13381594904270902445, ; 235: he\Microsoft.Maui.Controls.resources => 0xb9b4f9aaad3e94ad => 9
	i64 13454009404024712428, ; 236: Xamarin.Google.Guava.ListenableFuture => 0xbab63e4543a86cec => 90
	i64 13465488254036897740, ; 237: Xamarin.Kotlin.StdLib => 0xbadf06394d106fcc => 91
	i64 13467053111158216594, ; 238: uk/Microsoft.Maui.Controls.resources.dll => 0xbae49573fde79792 => 29
	i64 13540124433173649601, ; 239: vi\Microsoft.Maui.Controls.resources => 0xbbe82f6eede718c1 => 30
	i64 13545416393490209236, ; 240: id/Microsoft.Maui.Controls.resources.dll => 0xbbfafc7174bc99d4 => 13
	i64 13572454107664307259, ; 241: Xamarin.AndroidX.RecyclerView.dll => 0xbc5b0b19d99f543b => 84
	i64 13717397318615465333, ; 242: System.ComponentModel.Primitives.dll => 0xbe5dfc2ef2f87d75 => 105
	i64 13755568601956062840, ; 243: fr/Microsoft.Maui.Controls.resources.dll => 0xbee598c36b1b9678 => 8
	i64 13814445057219246765, ; 244: hr/Microsoft.Maui.Controls.resources.dll => 0xbfb6c49664b43aad => 11
	i64 13881769479078963060, ; 245: System.Console.dll => 0xc0a5f3cade5c6774 => 108
	i64 13959074834287824816, ; 246: Xamarin.AndroidX.Fragment => 0xc1b8989a7ad20fb0 => 74
	i64 14100563506285742564, ; 247: da/Microsoft.Maui.Controls.resources.dll => 0xc3af43cd0cff89e4 => 3
	i64 14124974489674258913, ; 248: Xamarin.AndroidX.CardView => 0xc405fd76067d19e1 => 67
	i64 14125464355221830302, ; 249: System.Threading.dll => 0xc407bafdbc707a9e => 153
	i64 14254574811015963973, ; 250: System.Text.Encoding.Extensions.dll => 0xc5d26c4442d66545 => 149
	i64 14461014870687870182, ; 251: System.Net.Requests.dll => 0xc8afd8683afdece6 => 129
	i64 14464374589798375073, ; 252: ru\Microsoft.Maui.Controls.resources => 0xc8bbc80dcb1e5ea1 => 24
	i64 14522721392235705434, ; 253: el/Microsoft.Maui.Controls.resources.dll => 0xc98b12295c2cf45a => 5
	i64 14556034074661724008, ; 254: CommunityToolkit.Maui.Core => 0xca016bdea6b19f68 => 37
	i64 14622043554576106986, ; 255: System.Runtime.Serialization.Formatters => 0xcaebef2458cc85ea => 144
	i64 14669215534098758659, ; 256: Microsoft.Extensions.DependencyInjection.dll => 0xcb9385ceb3993c03 => 42
	i64 14690985099581930927, ; 257: System.Web.HttpUtility => 0xcbe0dd1ca5233daf => 155
	i64 14705122255218365489, ; 258: ko\Microsoft.Maui.Controls.resources => 0xcc1316c7b0fb5431 => 16
	i64 14744092281598614090, ; 259: zh-Hans\Microsoft.Maui.Controls.resources => 0xcc9d89d004439a4a => 32
	i64 14792063746108907174, ; 260: Xamarin.Google.Guava.ListenableFuture.dll => 0xcd47f79af9c15ea6 => 90
	i64 14852515768018889994, ; 261: Xamarin.AndroidX.CursorAdapter.dll => 0xce1ebc6625a76d0a => 71
	i64 14892012299694389861, ; 262: zh-Hant/Microsoft.Maui.Controls.resources.dll => 0xceab0e490a083a65 => 33
	i64 14904040806490515477, ; 263: ar\Microsoft.Maui.Controls.resources => 0xced5ca2604cb2815 => 0
	i64 14935719434541007538, ; 264: System.Text.Encoding.CodePages.dll => 0xcf4655b160b702b2 => 148
	i64 14954917835170835695, ; 265: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xcf8a8a895a82ecef => 43
	i64 14987728460634540364, ; 266: System.IO.Compression.dll => 0xcfff1ba06622494c => 119
	i64 15015154896917945444, ; 267: System.Net.Security.dll => 0xd0608bd33642dc64 => 130
	i64 15076659072870671916, ; 268: System.ObjectModel.dll => 0xd13b0d8c1620662c => 133
	i64 15111608613780139878, ; 269: ms\Microsoft.Maui.Controls.resources => 0xd1b737f831192f66 => 17
	i64 15115185479366240210, ; 270: System.IO.Compression.Brotli.dll => 0xd1c3ed1c1bc467d2 => 118
	i64 15133485256822086103, ; 271: System.Linq.dll => 0xd204f0a9127dd9d7 => 123
	i64 15227001540531775957, ; 272: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd3512d3999b8e9d5 => 41
	i64 15370334346939861994, ; 273: Xamarin.AndroidX.Core.dll => 0xd54e65a72c560bea => 70
	i64 15391712275433856905, ; 274: Microsoft.Extensions.DependencyInjection.Abstractions => 0xd59a58c406411f89 => 43
	i64 15527772828719725935, ; 275: System.Console => 0xd77dbb1e38cd3d6f => 108
	i64 15536481058354060254, ; 276: de\Microsoft.Maui.Controls.resources => 0xd79cab34eec75bde => 4
	i64 15557562860424774966, ; 277: System.Net.Sockets => 0xd7e790fe7a6dc536 => 131
	i64 15582737692548360875, ; 278: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xd841015ed86f6aab => 78
	i64 15609085926864131306, ; 279: System.dll => 0xd89e9cf3334914ea => 159
	i64 15661133872274321916, ; 280: System.Xml.ReaderWriter.dll => 0xd9578647d4bfb1fc => 157
	i64 15664356999916475676, ; 281: de/Microsoft.Maui.Controls.resources.dll => 0xd962f9b2b6ecd51c => 4
	i64 15743187114543869802, ; 282: hu/Microsoft.Maui.Controls.resources.dll => 0xda7b09450ae4ef6a => 12
	i64 15750144475371186037, ; 283: Xamarin.AndroidX.Camera.View.dll => 0xda93c0f3d794a775 => 66
	i64 15783653065526199428, ; 284: el\Microsoft.Maui.Controls.resources => 0xdb0accd674b1c484 => 5
	i64 15928521404965645318, ; 285: Microsoft.Maui.Controls.Compatibility => 0xdd0d79d32c2eec06 => 48
	i64 16154507427712707110, ; 286: System => 0xe03056ea4e39aa26 => 159
	i64 16219561732052121626, ; 287: System.Net.Security => 0xe1177575db7c781a => 130
	i64 16274182383641215830, ; 288: zxing.dll => 0xe1d982a752dac356 => 93
	i64 16288847719894691167, ; 289: nb\Microsoft.Maui.Controls.resources => 0xe20d9cb300c12d5f => 18
	i64 16321164108206115771, ; 290: Microsoft.Extensions.Logging.Abstractions.dll => 0xe2806c487e7b0bbb => 45
	i64 16648892297579399389, ; 291: CommunityToolkit.Mvvm => 0xe70cbf55c4f508dd => 38
	i64 16649148416072044166, ; 292: Microsoft.Maui.Graphics => 0xe70da84600bb4e86 => 53
	i64 16677317093839702854, ; 293: Xamarin.AndroidX.Navigation.UI => 0xe771bb8960dd8b46 => 83
	i64 16755018182064898362, ; 294: SQLitePCLRaw.core.dll => 0xe885c843c330813a => 57
	i64 16833383113903931215, ; 295: mscorlib => 0xe99c30c1484d7f4f => 160
	i64 16856067890322379635, ; 296: System.Data.Common.dll => 0xe9ecc87060889373 => 110
	i64 16885326587688996227, ; 297: ZXing.Net.MAUI.dll => 0xea54bb11b7a94d83 => 94
	i64 16890310621557459193, ; 298: System.Text.RegularExpressions.dll => 0xea66700587f088f9 => 150
	i64 16891727762267860345, ; 299: Blanquita_Inventarios.Entities.dll => 0xea6b78e750946d79 => 96
	i64 16942731696432749159, ; 300: sk\Microsoft.Maui.Controls.resources => 0xeb20acb622a01a67 => 25
	i64 16998075588627545693, ; 301: Xamarin.AndroidX.Navigation.Fragment => 0xebe54bb02d623e5d => 81
	i64 17008137082415910100, ; 302: System.Collections.NonGeneric => 0xec090a90408c8cd4 => 102
	i64 17031351772568316411, ; 303: Xamarin.AndroidX.Navigation.Common.dll => 0xec5b843380a769fb => 80
	i64 17040771374769818752, ; 304: zxing => 0xec7cfb478bcccc80 => 93
	i64 17062143951396181894, ; 305: System.ComponentModel.Primitives => 0xecc8e986518c9786 => 105
	i64 17089008752050867324, ; 306: zh-Hans/Microsoft.Maui.Controls.resources.dll => 0xed285aeb25888c7c => 32
	i64 17230721278011714856, ; 307: System.Private.Xml.Linq => 0xef1fd1b5c7a72d28 => 135
	i64 17234219099804750107, ; 308: System.Transactions.Local.dll => 0xef2c3ef5e11d511b => 154
	i64 17260702271250283638, ; 309: System.Data.Common => 0xef8a5543bba6bc76 => 110
	i64 17306917412052548875, ; 310: ZXing.Net.MAUI => 0xf02e85b0b66a3d0b => 94
	i64 17333249706306540043, ; 311: System.Diagnostics.Tracing.dll => 0xf08c12c5bb8b920b => 114
	i64 17342750010158924305, ; 312: hi\Microsoft.Maui.Controls.resources => 0xf0add33f97ecc211 => 10
	i64 17411438186820774435, ; 313: Controls.UserDialogs.Maui.dll => 0xf1a1dac7fa1faa23 => 39
	i64 17438153253682247751, ; 314: sk/Microsoft.Maui.Controls.resources.dll => 0xf200c3fe308d7847 => 25
	i64 17514990004910432069, ; 315: fr\Microsoft.Maui.Controls.resources => 0xf311be9c6f341f45 => 8
	i64 17623389608345532001, ; 316: pl\Microsoft.Maui.Controls.resources => 0xf492db79dfbef661 => 20
	i64 17702523067201099846, ; 317: zh-HK/Microsoft.Maui.Controls.resources.dll => 0xf5abfef008ae1846 => 31
	i64 17704177640604968747, ; 318: Xamarin.AndroidX.Loader => 0xf5b1dfc36cac272b => 79
	i64 17710060891934109755, ; 319: Xamarin.AndroidX.Lifecycle.ViewModel => 0xf5c6c68c9e45303b => 77
	i64 17712670374920797664, ; 320: System.Runtime.InteropServices.dll => 0xf5d00bdc38bd3de0 => 141
	i64 17777860260071588075, ; 321: System.Runtime.Numerics.dll => 0xf6b7a5b72419c0eb => 143
	i64 18025913125965088385, ; 322: System.Threading => 0xfa28e87b91334681 => 153
	i64 18099568558057551825, ; 323: nl/Microsoft.Maui.Controls.resources.dll => 0xfb2e95b53ad977d1 => 19
	i64 18121036031235206392, ; 324: Xamarin.AndroidX.Navigation.Common => 0xfb7ada42d3d42cf8 => 80
	i64 18146411883821974900, ; 325: System.Formats.Asn1.dll => 0xfbd50176eb22c574 => 117
	i64 18225059387460068507, ; 326: System.Threading.ThreadPool.dll => 0xfcec6af3cff4a49b => 152
	i64 18245806341561545090, ; 327: System.Collections.Concurrent.dll => 0xfd3620327d587182 => 100
	i64 18305135509493619199, ; 328: Xamarin.AndroidX.Navigation.Runtime.dll => 0xfe08e7c2d8c199ff => 82
	i64 18324163916253801303, ; 329: it\Microsoft.Maui.Controls.resources => 0xfe4c81ff0a56ab57 => 14
	i64 18335459783622540540, ; 330: ZXing.Net.MAUI.Controls => 0xfe74a3871c483cfc => 95
	i64 18370042311372477656 ; 331: SQLitePCLRaw.lib.e_sqlite3.android.dll => 0xfeef80274e4094d8 => 58
], align 8

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [332 x i32] [
	i32 47, ; 0
	i32 165, ; 1
	i32 52, ; 2
	i32 121, ; 3
	i32 68, ; 4
	i32 85, ; 5
	i32 7, ; 6
	i32 139, ; 7
	i32 96, ; 8
	i32 151, ; 9
	i32 107, ; 10
	i32 10, ; 11
	i32 58, ; 12
	i32 73, ; 13
	i32 139, ; 14
	i32 89, ; 15
	i32 18, ; 16
	i32 116, ; 17
	i32 57, ; 18
	i32 81, ; 19
	i32 128, ; 20
	i32 49, ; 21
	i32 164, ; 22
	i32 59, ; 23
	i32 151, ; 24
	i32 16, ; 25
	i32 63, ; 26
	i32 78, ; 27
	i32 54, ; 28
	i32 126, ; 29
	i32 124, ; 30
	i32 134, ; 31
	i32 62, ; 32
	i32 140, ; 33
	i32 6, ; 34
	i32 85, ; 35
	i32 115, ; 36
	i32 28, ; 37
	i32 50, ; 38
	i32 38, ; 39
	i32 28, ; 40
	i32 77, ; 41
	i32 2, ; 42
	i32 20, ; 43
	i32 115, ; 44
	i32 54, ; 45
	i32 73, ; 46
	i32 100, ; 47
	i32 24, ; 48
	i32 76, ; 49
	i32 69, ; 50
	i32 146, ; 51
	i32 61, ; 52
	i32 27, ; 53
	i32 160, ; 54
	i32 127, ; 55
	i32 42, ; 56
	i32 2, ; 57
	i32 7, ; 58
	i32 89, ; 59
	i32 75, ; 60
	i32 132, ; 61
	i32 143, ; 62
	i32 131, ; 63
	i32 92, ; 64
	i32 52, ; 65
	i32 40, ; 66
	i32 86, ; 67
	i32 162, ; 68
	i32 22, ; 69
	i32 146, ; 70
	i32 41, ; 71
	i32 157, ; 72
	i32 40, ; 73
	i32 161, ; 74
	i32 84, ; 75
	i32 44, ; 76
	i32 50, ; 77
	i32 129, ; 78
	i32 124, ; 79
	i32 145, ; 80
	i32 136, ; 81
	i32 48, ; 82
	i32 33, ; 83
	i32 107, ; 84
	i32 152, ; 85
	i32 121, ; 86
	i32 106, ; 87
	i32 56, ; 88
	i32 30, ; 89
	i32 120, ; 90
	i32 155, ; 91
	i32 60, ; 92
	i32 0, ; 93
	i32 65, ; 94
	i32 61, ; 95
	i32 86, ; 96
	i32 116, ; 97
	i32 127, ; 98
	i32 142, ; 99
	i32 103, ; 100
	i32 103, ; 101
	i32 60, ; 102
	i32 142, ; 103
	i32 140, ; 104
	i32 26, ; 105
	i32 109, ; 106
	i32 29, ; 107
	i32 118, ; 108
	i32 147, ; 109
	i32 88, ; 110
	i32 117, ; 111
	i32 23, ; 112
	i32 56, ; 113
	i32 23, ; 114
	i32 154, ; 115
	i32 137, ; 116
	i32 34, ; 117
	i32 76, ; 118
	i32 11, ; 119
	i32 72, ; 120
	i32 46, ; 121
	i32 148, ; 122
	i32 19, ; 123
	i32 22, ; 124
	i32 113, ; 125
	i32 158, ; 126
	i32 156, ; 127
	i32 132, ; 128
	i32 95, ; 129
	i32 135, ; 130
	i32 97, ; 131
	i32 97, ; 132
	i32 26, ; 133
	i32 123, ; 134
	i32 99, ; 135
	i32 106, ; 136
	i32 156, ; 137
	i32 133, ; 138
	i32 112, ; 139
	i32 125, ; 140
	i32 126, ; 141
	i32 17, ; 142
	i32 162, ; 143
	i32 98, ; 144
	i32 91, ; 145
	i32 63, ; 146
	i32 35, ; 147
	i32 104, ; 148
	i32 75, ; 149
	i32 111, ; 150
	i32 163, ; 151
	i32 104, ; 152
	i32 53, ; 153
	i32 112, ; 154
	i32 66, ; 155
	i32 147, ; 156
	i32 83, ; 157
	i32 138, ; 158
	i32 101, ; 159
	i32 134, ; 160
	i32 36, ; 161
	i32 21, ; 162
	i32 163, ; 163
	i32 64, ; 164
	i32 98, ; 165
	i32 82, ; 166
	i32 21, ; 167
	i32 111, ; 168
	i32 158, ; 169
	i32 31, ; 170
	i32 64, ; 171
	i32 88, ; 172
	i32 62, ; 173
	i32 150, ; 174
	i32 69, ; 175
	i32 102, ; 176
	i32 35, ; 177
	i32 165, ; 178
	i32 122, ; 179
	i32 51, ; 180
	i32 6, ; 181
	i32 125, ; 182
	i32 136, ; 183
	i32 49, ; 184
	i32 3, ; 185
	i32 72, ; 186
	i32 138, ; 187
	i32 137, ; 188
	i32 92, ; 189
	i32 70, ; 190
	i32 1, ; 191
	i32 99, ; 192
	i32 128, ; 193
	i32 122, ; 194
	i32 36, ; 195
	i32 114, ; 196
	i32 45, ; 197
	i32 51, ; 198
	i32 109, ; 199
	i32 12, ; 200
	i32 87, ; 201
	i32 15, ; 202
	i32 46, ; 203
	i32 71, ; 204
	i32 141, ; 205
	i32 13, ; 206
	i32 87, ; 207
	i32 44, ; 208
	i32 161, ; 209
	i32 9, ; 210
	i32 55, ; 211
	i32 55, ; 212
	i32 145, ; 213
	i32 101, ; 214
	i32 59, ; 215
	i32 37, ; 216
	i32 74, ; 217
	i32 79, ; 218
	i32 34, ; 219
	i32 113, ; 220
	i32 67, ; 221
	i32 119, ; 222
	i32 14, ; 223
	i32 68, ; 224
	i32 149, ; 225
	i32 39, ; 226
	i32 65, ; 227
	i32 27, ; 228
	i32 120, ; 229
	i32 47, ; 230
	i32 144, ; 231
	i32 1, ; 232
	i32 15, ; 233
	i32 164, ; 234
	i32 9, ; 235
	i32 90, ; 236
	i32 91, ; 237
	i32 29, ; 238
	i32 30, ; 239
	i32 13, ; 240
	i32 84, ; 241
	i32 105, ; 242
	i32 8, ; 243
	i32 11, ; 244
	i32 108, ; 245
	i32 74, ; 246
	i32 3, ; 247
	i32 67, ; 248
	i32 153, ; 249
	i32 149, ; 250
	i32 129, ; 251
	i32 24, ; 252
	i32 5, ; 253
	i32 37, ; 254
	i32 144, ; 255
	i32 42, ; 256
	i32 155, ; 257
	i32 16, ; 258
	i32 32, ; 259
	i32 90, ; 260
	i32 71, ; 261
	i32 33, ; 262
	i32 0, ; 263
	i32 148, ; 264
	i32 43, ; 265
	i32 119, ; 266
	i32 130, ; 267
	i32 133, ; 268
	i32 17, ; 269
	i32 118, ; 270
	i32 123, ; 271
	i32 41, ; 272
	i32 70, ; 273
	i32 43, ; 274
	i32 108, ; 275
	i32 4, ; 276
	i32 131, ; 277
	i32 78, ; 278
	i32 159, ; 279
	i32 157, ; 280
	i32 4, ; 281
	i32 12, ; 282
	i32 66, ; 283
	i32 5, ; 284
	i32 48, ; 285
	i32 159, ; 286
	i32 130, ; 287
	i32 93, ; 288
	i32 18, ; 289
	i32 45, ; 290
	i32 38, ; 291
	i32 53, ; 292
	i32 83, ; 293
	i32 57, ; 294
	i32 160, ; 295
	i32 110, ; 296
	i32 94, ; 297
	i32 150, ; 298
	i32 96, ; 299
	i32 25, ; 300
	i32 81, ; 301
	i32 102, ; 302
	i32 80, ; 303
	i32 93, ; 304
	i32 105, ; 305
	i32 32, ; 306
	i32 135, ; 307
	i32 154, ; 308
	i32 110, ; 309
	i32 94, ; 310
	i32 114, ; 311
	i32 10, ; 312
	i32 39, ; 313
	i32 25, ; 314
	i32 8, ; 315
	i32 20, ; 316
	i32 31, ; 317
	i32 79, ; 318
	i32 77, ; 319
	i32 141, ; 320
	i32 143, ; 321
	i32 153, ; 322
	i32 19, ; 323
	i32 80, ; 324
	i32 117, ; 325
	i32 152, ; 326
	i32 100, ; 327
	i32 82, ; 328
	i32 14, ; 329
	i32 95, ; 330
	i32 58 ; 331
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 8

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 8

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 8

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
	store ptr %fn, ptr @get_function_pointer, align 8, !tbaa !3
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
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+fix-cortex-a53-835769,+neon,+outline-atomics,+v8a" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+fix-cortex-a53-835769,+neon,+outline-atomics,+v8a" }

; Metadata
!llvm.module.flags = !{!0, !1, !7, !8, !9, !10}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.4xx @ 82d8938cf80f6d5fa6c28529ddfbdb753d805ab4"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"branch-target-enforcement", i32 0}
!8 = !{i32 1, !"sign-return-address", i32 0}
!9 = !{i32 1, !"sign-return-address-all", i32 0}
!10 = !{i32 1, !"sign-return-address-with-bkey", i32 0}
