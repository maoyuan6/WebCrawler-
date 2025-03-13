using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMR;

namespace WindowsFormsApp1.Dapper
{
    public class EmrTempletRepositories
    {
        DapperHelper EMRContext = new DapperHelper("EMR");

        public List<EmrTemplet> GeEmrTempletList()
        {
            var sql = "select* from [EMR].[EmrTemplet]";
            var list = EMRContext.Query<EmrTemplet>(sql).ToList();
            return list;
        }

        public int InsertEmrTemplet(EmrTemplet emrTemplet)
        {
            var sql = @"
        INSERT INTO [EMR].[EMRTEMPLET] (
            TEMPLET_ID, FILE_NAME, DEPT_ID, CREATOR_ID, CREATE_DATETIME, LAST_TIME, PERMISSION,
            MR_CLASS, MR_CODE, MR_NAME, MR_ATTR, QC_CODE, NEW_PAGE_FLAG, FILE_FLAG, WRITE_TIMES,
            CODE, HOSPITAL_CODE, XML_DOC, XML_DOC_NEW, PY, WB, ISFIRSTDAILY, ISSHOWFILENAME,
            ISYIHUANGOUTONG, NEW_PAGE_END, VALID, STATE, AUDITOR, AUDITDATE, ISCONFIGPAGESIZE,
            ZYZHENDUAN, XYZHENDUAN
        ) 
        VALUES (
            @TEMPLET_ID, @FILE_NAME, @DEPT_ID, @CREATOR_ID, @CREATE_DATETIME, @LAST_TIME, @PERMISSION,
            @MR_CLASS, @MR_CODE, @MR_NAME, @MR_ATTR, @QC_CODE, @NEW_PAGE_FLAG, @FILE_FLAG, @WRITE_TIMES,
            @CODE, @HOSPITAL_CODE, @XML_DOC, @XML_DOC_NEW, @PY, @WB, @ISFIRSTDAILY, @ISSHOWFILENAME,
            @ISYIHUANGOUTONG, @NEW_PAGE_END, @VALID, @STATE, @AUDITOR, @AUDITDATE, @ISCONFIGPAGESIZE,
            @ZYZHENDUAN, @XYZHENDUAN
        );";

            return EMRContext.Insert<int>(sql, emrTemplet);
        }

        /// <summary>
        /// 更新 EmrTemplet 表中的数据
        /// </summary>
        /// <param name="emrTemplet">要更新的数据对象</param>
        public int UpdateEmrTemplet(EmrTemplet emrTemplet)
        {
            var sql = @"
        UPDATE [EMR].[EMRTEMPLET]
        SET 
            FILE_NAME          = @FILE_NAME,
            DEPT_ID            = @DEPT_ID,
            CREATOR_ID         = @CREATOR_ID,
            CREATE_DATETIME    = @CREATE_DATETIME,
            LAST_TIME          = @LAST_TIME,
            PERMISSION         = @PERMISSION,
            MR_CLASS           = @MR_CLASS,
            MR_CODE            = @MR_CODE,
            MR_NAME            = @MR_NAME,
            MR_ATTR            = @MR_ATTR,
            QC_CODE            = @QC_CODE,
            NEW_PAGE_FLAG      = @NEW_PAGE_FLAG,
            FILE_FLAG          = @FILE_FLAG,
            WRITE_TIMES        = @WRITE_TIMES,
            CODE               = @CODE,
            HOSPITAL_CODE      = @HOSPITAL_CODE,
            XML_DOC            = @XML_DOC,
            XML_DOC_NEW        = @XML_DOC_NEW,
            PY                 = @PY,
            WB                 = @WB,
            ISFIRSTDAILY       = @ISFIRSTDAILY,
            ISSHOWFILENAME     = @ISSHOWFILENAME,
            ISYIHUANGOUTONG    = @ISYIHUANGOUTONG,
            NEW_PAGE_END       = @NEW_PAGE_END,
            VALID              = @VALID,
            STATE              = @STATE,
            AUDITOR            = @AUDITOR,
            AUDITDATE          = @AUDITDATE,
            ISCONFIGPAGESIZE   = @ISCONFIGPAGESIZE,
            ZYZHENDUAN         = @ZYZHENDUAN,
            XYZHENDUAN         = @XYZHENDUAN
        WHERE TEMPLET_ID = @TEMPLET_ID";

            return EMRContext.Execute(sql, emrTemplet);
        }
    }
}
