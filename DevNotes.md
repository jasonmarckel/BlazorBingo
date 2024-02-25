.NET 8 Blazor WASM introduced the use of SIMD and BigInt / BigInt64Array which is not supported
on older devices such as iPad Air (1st gen) or iPhone 6 running iOS 12.5.7.

There is an option for Blazor WASM client .csproj file to disable use of SIMD.

I added BigInt64ArrayPolyfill.js to get past the assert by dotnet.js to check for support.  This got the loading to go from 0 to 100, but then the WASM portion hangs.

.csproj
	<!-- workaround for older versions of IOS devices which lack SIMD support -->
	<RunAOTCompilation>false</RunAOTCompilation>
	<WasmEnableSIMD>false</WasmEnableSIMD>

index.html

    <!-- workaround for IOS -->
    <!--<script>var Module;</script>-->
    <!--<script type="module">
        import jsbi from 'https://cdn.jsdelivr.net/npm/jsbi@4.3.0/+esm'
    </script>-->
    <!--
        https://github.com/dotnet/runtime/blob/main/src/mono/wasm/features.md
        https://www.jsdelivr.com/package/npm/jsbi
        https://github.com/GoogleChromeLabs/jsbi/tree/main
    -->
    <script type="module">
        import * as JSBI from 'https://cdn.jsdelivr.net/npm/jsbi@4.3.0/dist/jsbi-umd.min.js'
    </script>
    <script src="script/BigInt64ArrayPolyfill.js"></script>

script/BigInt64ArrayPolyfill.js
    https://github.com/ivanrhsosa/sample-onnx/blob/main/shim.js
    comment out line to require JSBI
    //const JSBI = require('jsbi').default;
