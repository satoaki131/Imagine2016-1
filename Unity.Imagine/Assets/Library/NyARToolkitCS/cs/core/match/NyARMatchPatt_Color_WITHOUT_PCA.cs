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
using System.Diagnostics;
namespace NyAR.Core
{




    /**
     * このクラスは、カラーで２パターンの一致度を計算します。
     * 評価アルゴリズムは、ARToolKitの、AR_TEMPLATE_MATCHING_COLORかつAR_MATCHING_WITHOUT_PCAと同様です。
     * 比較対象のデータには、{@link NyARMatchPattDeviationColorData}クラスの物を使います。
     */
    public class NyARMatchPatt_Color_WITHOUT_PCA : INyARMatchPatt
    {
        /**　基準パターンへの産初値*/
        protected NyARCode _code_patt;
        /** 最適化定数*/
        protected int _optimize_for_mod;
        /** 最適化定数*/
        protected int _rgbpixels;
        /**
         * コンストラクタ。
         * 基準パターンを元に、インスタンスを生成します。
         * @param i_code_ref
         * セットする基準パターンの参照値
         */
        public NyARMatchPatt_Color_WITHOUT_PCA(NyARCode i_code_ref)
        {
            int w = i_code_ref.getWidth();
            int h = i_code_ref.getHeight();
            //最適化定数の計算
            this._rgbpixels = w * h * 3;
            this._optimize_for_mod = this._rgbpixels - (this._rgbpixels % 16);
            this.setARCode(i_code_ref);
            return;
        }
        /**
         * コンストラクタ。
         * 基準パターンの解像度を指定して、インスタンスを生成します。
         * このコンストラクタで生成したインスタンスの基準パターンは、NULLになっています。
         * 後で基準パターンを{@link #setARCode}関数で設定してください。
         * @param i_width
         * 基準パターンのサイズ
         * @param i_height
         * 基準パターンのサイズ
         */
        public NyARMatchPatt_Color_WITHOUT_PCA(int i_width, int i_height)
        {
            //最適化定数の計算
            this._rgbpixels = i_height * i_width * 3;
            this._optimize_for_mod = this._rgbpixels - (this._rgbpixels % 16);
            return;
        }
        /**
         * 基準パターンをセットします。セットできる基準パターンは、コンストラクタに設定したサイズと同じものである必要があります。
         * @param i_code_ref
         * セットする基準パターンを格納したオブジェクト
         * @
         */
        public void setARCode(NyARCode i_code_ref)
        {
            this._code_patt = i_code_ref;
            return;
        }
        /**
         * この関数は、現在の基準パターンと検査パターンを比較して、類似度を計算します。
         * @param i_patt
         * 検査パターンを格納したオブジェクトです。このサイズは、基準パターンと一致している必要があります。
         * @param o_result
         * 結果を受け取るオブジェクトです。
         * @return
         * 検査に成功するとtrueを返します。
         * @
         */
        public bool evaluate(NyARMatchPattDeviationColorData i_patt, NyARMatchPattResult o_result)
        {
            Debug.Assert(this._code_patt != null);
            //
            int[] linput = i_patt.getData();
            int sum;
            double max = double.MinValue;
            int res = NyARMatchPattResult.DIRECTION_UNKNOWN;
            int for_mod = this._optimize_for_mod;
            for (int j = 0; j < 4; j++)
            {
                //合計値初期化
                sum = 0;
                NyARMatchPattDeviationColorData code_patt = this._code_patt.getColorData(j);
                int[] pat_j = code_patt.getData();
                //<全画素について、比較(FORの1/16展開)>
                int i;
                for (i = this._rgbpixels - 1; i >= for_mod; i--)
                {
                    sum += linput[i] * pat_j[i];
                }
                for (; i >= 0; )
                {
                    sum += linput[i] * pat_j[i]; i--;
                    sum += linput[i] * pat_j[i]; i--;
                    sum += linput[i] * pat_j[i]; i--;
                    sum += linput[i] * pat_j[i]; i--;
                    sum += linput[i] * pat_j[i]; i--;
                    sum += linput[i] * pat_j[i]; i--;
                    sum += linput[i] * pat_j[i]; i--;
                    sum += linput[i] * pat_j[i]; i--;
                    sum += linput[i] * pat_j[i]; i--;
                    sum += linput[i] * pat_j[i]; i--;
                    sum += linput[i] * pat_j[i]; i--;
                    sum += linput[i] * pat_j[i]; i--;
                    sum += linput[i] * pat_j[i]; i--;
                    sum += linput[i] * pat_j[i]; i--;
                    sum += linput[i] * pat_j[i]; i--;
                    sum += linput[i] * pat_j[i]; i--;
                }
                //<全画素について、比較(FORの1/16展開)/>
                double sum2 = sum / code_patt.getPow();// sum2 = sum / patpow[k][j]/ datapow;
                if (sum2 > max)
                {
                    max = sum2;
                    res = j;
                }
            }
            o_result.direction = res;
            o_result.confidence = max / i_patt.getPow();
            return true;
        }
    }
}