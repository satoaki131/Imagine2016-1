﻿
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

using NyAR.Core;

namespace NyAR.Rpf {
  public class VecLinearCoordinatesOperator {

    /**
     * margeResembleCoordsで使う距離敷居値の値です。
     * 許容する((距離^2)*2)を指定します。
     */
    private const int _SQ_DIFF_DOT_TH = ((10 * 10) * 2);

    /**
     * margeResembleCoordsで使う角度敷居値の値です。
     * Cos(n)の値です。
     */
    private const double _SQ_ANG_TH = NyARMath.COS_DEG_10;

    //ワーク
    private NyARLinear _l1 = new NyARLinear();
    private NyARLinear _l2 = new NyARLinear();
    private NyARDoublePoint2d _p = new NyARDoublePoint2d();

    /**
     * 配列の前方に、似たベクトルを集めます。似たベクトルの判定基準は、2線の定義点における直線の法線上での距離の二乗和です。
     * ベクトルの統合と位置情報の計算には、加重平均を用います。
     * @param i_vector
     * 編集するオブジェクトを指定します。
     */
    public void MargeResembleCoords(VecLinearCoordinates i_vector) {
      VecLinearCoordinates.VecLinearCoordinatePoint[] items = i_vector.items;
      NyARLinear l1 = this._l1;
      NyARLinear l2 = this._l2;
      NyARDoublePoint2d p = this._p;

      for (int i = i_vector.length - 1; i >= 0; i--) {
        VecLinearCoordinates.VecLinearCoordinatePoint target1 = items[i];
        if (target1.scalar == 0) { continue; }

        double rdx = target1.dx;
        double rdy = target1.dy;
        double rx = target1.x;
        double ry = target1.y;
        l1.setVector(target1);
        double s_tmp = target1.scalar;
        target1.dx *= s_tmp;
        target1.dy *= s_tmp;
        target1.x *= s_tmp;
        target1.y *= s_tmp;

        for (int i2 = i - 1; i2 >= 0; i2--) {
          VecLinearCoordinates.VecLinearCoordinatePoint target2 = items[i2];
          if (target2.scalar == 0) { continue; }

          if (target2.getVecCos(rdx, rdy) >= _SQ_ANG_TH) {
            // それぞれの代表点から法線を引いて、相手の直線との交点を計算する。
            l2.setVector(target2);
            l1.normalLineCrossPos(rx, ry, l2, p);
            double wx, wy;
            double l = 0;
            // 交点間の距離の合計を計算。lに2*dist^2を得る。
            wx = (p.x - rx);
            wy = (p.y - ry);
            l += wx * wx + wy * wy;
            l2.normalLineCrossPos(target2.x, target2.y, l2, p);
            wx = (p.x - target2.x);
            wy = (p.y - target2.y);
            l += wx * wx + wy * wy;

            // 距離が一定値以下なら、マージ
            if (l > _SQ_DIFF_DOT_TH) { continue; }

            // 似たようなベクトル発見したら、後方のアイテムに値を統合。
            s_tmp = target2.scalar;
            target1.x += target2.x * s_tmp;
            target1.y += target2.y * s_tmp;
            target1.dx += target2.dx * s_tmp;
            target1.dy += target2.dy * s_tmp;
            target1.scalar += s_tmp;

            //要らない子を無効化しておく。
            target2.scalar = 0;
          }
        }
      }
      //前方詰め
      i_vector.removeZeroDistItem();
      //加重平均解除なう(x,y位置のみ)
      for (int i = 0; i < i_vector.length; i++) {
        VecLinearCoordinates.VecLinearCoordinatePoint ptr = items[i];
        double d = 1 / ptr.scalar;
        ptr.x *= d;
        ptr.y *= d;
        ptr.dx *= d;
        ptr.dy *= d;
      }
    }
  }
}
