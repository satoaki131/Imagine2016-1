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
using NyAR.Core;

namespace NyAR.Rpf
{

    /**
     * TargetStatusの基礎クラスです。TargetStatusは、ステータス毎に異なるターゲットのパラメータを格納します。
     * @note
     * ST_から始まるID値は、NyARTrackerのコンストラクタと密接に絡んでいるので、変更するときは気をつけて！
     *
     */
    public class NyARTargetStatus : NyARManagedObject
    {
	    public const int ST_IGNORE=0;
	    public const int ST_NEW=1;
	    public const int ST_RECT=2;
	    public const int ST_CONTURE=3;
	    public const int MAX_OF_ST_KIND=3;
	    protected NyARTargetStatus(INyARManagedObjectPoolOperater iRefPoolOperator)
            : base(iRefPoolOperator)
	    {
	    }
    }
}