/* 
 * PROJECT: NyARToolkitCS
 * --------------------------------------------------------------------------------
 *
 * The NyARToolkitCS is C# edition NyARToolKit class library.
 * Copyright (C)2008-2012 Ryo Iizuka
 *
 * This work is based on the ARToolKit developed by
 *   Hirokazu Kato
 *   Mark Billinghurst
 *   HITLab, University of Washington, Seattle
 * http://www.hitl.washington.edu/artoolkit/
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as publishe
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
 * For further information please contact.
 *	http://nyatla.jp/nyatoolkit/
 *	<airmail(at)ebony.plala.or.jp> or <nyatla(at)nyatla.jp>
 * 
 */
using System;
using System.Diagnostics;
namespace NyAR.Core
{

    /**
     * このクラスは、指定形式のバッファを持つRGBラスタです。
     * 外部参照バッファ、内部バッファの両方に対応します。
     * <p>
     * 対応しているバッファタイプ-
     * <ul>{@link NyARBufferType#INT1D_X8R8G8B8_32}
     * <li>{@link NyARBufferType#BYTE1D_B8G8R8X8_32}
     * <li>{@link NyARBufferType#BYTE1D_R8G8B8_24}
     * <li>{@link NyARBufferType#BYTE1D_B8G8R8_24}
     * <li>{@link NyARBufferType#BYTE1D_X8R8G8B8_32}
     * <li>{@link NyARBufferType#WORD1D_R5G6B5_16LE}
     * </ul>
     * </p>
     */
    public class NyARRgbRaster : NyARRgbRaster_BasicClass
    {
        /** バッファオブジェクト*/
        protected object _buf;
        /** ピクセルリーダ*/
        protected INyARRgbPixelDriver _rgb_pixel_driver;
        /** バッファオブジェクトがアタッチされていればtrue*/
        protected bool _is_attached_buffer;

        /**
         * コンストラクタです。
         * 画像のサイズパラメータとバッファ形式を指定して、インスタンスを生成します。
         * @param i_width
         * ラスタのサイズ
         * @param i_height
         * ラスタのサイズ
         * @param i_raster_type
         * ラスタのバッファ形式。
         * {@link NyARBufferType}に定義された定数値を指定してください。
         * 指定できる値は、クラスの説明を見てください。
         * @param i_is_alloc
         * バッファを外部参照にするかのフラグ値。
         * trueなら内部バッファ、falseなら外部バッファを使用します。
         * falseの場合、初期のバッファはnullになります。インスタンスを生成したのちに、{@link #wrapBuffer}を使って割り当ててください。
         * @
         */
        public NyARRgbRaster(int i_width, int i_height, int i_raster_type, bool i_is_alloc):base(i_width, i_height, i_raster_type)
        {
            InitInstance(this._size, i_raster_type, i_is_alloc);
        }
        /**
         * コンストラクタです。
         * 画像のサイズパラメータとバッファ形式を指定して、インスタンスを生成します。
         * @param i_width
         * ラスタのサイズ
         * @param i_height
         * ラスタのサイズ
         * @param i_raster_type
         * ラスタのバッファ形式。
         * {@link NyARBufferType}に定義された定数値を指定してください。
         * 指定できる値は、クラスの説明を見てください。
         * @
         */
        public NyARRgbRaster(int i_width, int i_height, int i_raster_type):base(i_width, i_height, i_raster_type)
        {
            InitInstance(this._size, i_raster_type, true);
        }
        /**
         * コンストラクタです。
         * 画像サイズを指定してインスタンスを生成します。
         * @param i_width
         * ラスタのサイズ
         * @param i_height
         * ラスタのサイズ
         * @
         */
        public NyARRgbRaster(int i_width, int i_height): base(i_width, i_height, NyARBufferType.INT1D_X8R8G8B8_32)
        {
            InitInstance(this._size, NyARBufferType.INT1D_X8R8G8B8_32, true);
        }
        /**
         * Readerとbufferを初期化する関数です。コンストラクタから呼び出します。
         * 継承クラスでこの関数を拡張することで、対応するバッファタイプの種類を増やせます。
         * @param i_size
         * ラスタのサイズ
         * @param i_raster_type
         * バッファタイプ
         * @param i_is_alloc
         * 外部参照/内部バッファのフラグ
         * @return
         * 初期化が成功すると、trueです。
         * @ 
         */
        protected virtual void InitInstance(NyARIntSize i_size, int i_raster_type, bool i_is_alloc)
        {
            //バッファの構築
            switch (i_raster_type)
            {
                case NyARBufferType.INT1D_X8R8G8B8_32:
                    this._buf = i_is_alloc ? new int[i_size.w * i_size.h] : null;
                    break;
                case NyARBufferType.BYTE1D_B8G8R8X8_32:
                case NyARBufferType.BYTE1D_X8R8G8B8_32:
                    this._buf = i_is_alloc ? new byte[i_size.w * i_size.h * 4] : null;
                    break;
                case NyARBufferType.BYTE1D_R8G8B8_24:
                case NyARBufferType.BYTE1D_B8G8R8_24:
                    this._buf = i_is_alloc ? new byte[i_size.w * i_size.h * 3] : null;
                    break;
                case NyARBufferType.WORD1D_R5G6B5_16LE:
                    this._buf = i_is_alloc ? new short[i_size.w * i_size.h] : null;
                    break;
                default:
                    throw new NyARException();
            }
            //readerの構築
            this._rgb_pixel_driver = NyARRgbPixelDriverFactory.createDriver(this);
            this._is_attached_buffer = i_is_alloc;
            return;
        }
        /**
         * この関数は、画素形式によらない画素アクセスを行うオブジェクトへの参照値を返します。
         * @return
         * オブジェクトの参照値
         * @
         */
        public override INyARRgbPixelDriver getRgbPixelDriver()
        {
            return this._rgb_pixel_driver;
        }
        /**
         * この関数は、ラスタのバッファへの参照値を返します。
         * バッファの形式は、コンストラクタに指定した形式と同じです。
         */
        public override object getBuffer()
        {
            return this._buf;
        }
        /**
         * インスタンスがバッファを所有するかを返します。
         * コンストラクタでi_is_allocをfalseにしてラスタを作成した場合、
         * バッファにアクセスするまえに、バッファの有無をこの関数でチェックしてください。
         * @return
         * インスタンスがバッファを所有すれば、trueです。
         */
        public override bool hasBuffer()
        {
            return this._buf != null;
        }
        /**
         * この関数は、ラスタに外部参照バッファをセットします。
         * 外部参照バッファの時にだけ使えます。
         */
        public override void WrapBuffer(object i_ref_buf)
        {
            Debug.Assert(!this._is_attached_buffer);//バッファがアタッチされていたら機能しない。
            this._buf = i_ref_buf;
            //ピクセルリーダーの参照バッファを切り替える。
            this._rgb_pixel_driver.switchRaster(this);
        }
        public override object createInterface(Type iIid)
        {
            if (iIid == typeof(INyARPerspectiveCopy))
            {
                return NyARPerspectiveCopyFactory.createDriver(this);
            }
            if (iIid == typeof(NyARMatchPattDeviationColorData.IRasterDriver))
            {
                return NyARMatchPattDeviationColorData.RasterDriverFactory.createDriver(this);
            }
            if (iIid == typeof(INyARRgb2GsFilter))
            {
                //デフォルトのインタフェイス
                return NyARRgb2GsFilterFactory.createRgbAveDriver(this);
            }
            else if (iIid == typeof(INyARRgb2GsFilterRgbAve))
            {
                return NyARRgb2GsFilterFactory.createRgbAveDriver(this);
            }
            else if (iIid == typeof(INyARRgb2GsFilterRgbCube))
            {
                return NyARRgb2GsFilterFactory.createRgbCubeDriver(this);
            }
            else if (iIid == typeof(INyARRgb2GsFilterYCbCr))
            {
                return NyARRgb2GsFilterFactory.createYCbCrDriver(this);
            }
            if (iIid == typeof(INyARRgb2GsFilterArtkTh))
            {
                return NyARRgb2GsFilterArtkThFactory.createDriver(this);
            }
            throw new NyARException();
        }
    }
}
