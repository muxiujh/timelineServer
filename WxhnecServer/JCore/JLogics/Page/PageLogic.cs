using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JCore
{
    public class PageLogic
    {
        readonly int m_first = 1;
        readonly string m_next = "下一页";
        readonly string m_more = "...";

        // input
        int m_size;
        int m_total;
        int m_pageIndex;
        int m_pageShow;

        // calc
        int m_pageTotal;
        int m_pageStart;
        int m_pageEnd;

        public PageLogic(SPage spage) {
            m_pageIndex = spage.Index;
            m_size = spage.Size;
            m_total = spage.Total;
            m_pageShow = spage.PageShow;
            calc();
        }
        
        void calc() {
            m_pageTotal = (int)Math.Ceiling((float)m_total / m_size);
            if (m_pageIndex > m_pageTotal) {
                m_pageIndex = m_pageTotal;
            }
            else if(m_pageIndex < m_first) {
                m_pageIndex = m_first;
            }

            if (m_pageShow >= m_pageTotal) {
                // show all
                m_pageStart = m_first;
                m_pageEnd = m_pageTotal;
            }
            else {
                int pageMove = m_pageShow / 2;

                if (m_pageIndex - pageMove <= m_first) {
                    // first
                    m_pageStart = m_first;
                }
                else if (m_pageIndex + pageMove >= m_pageTotal) {
                    // last
                    m_pageStart = m_pageTotal - m_pageShow + 1;
                }
                else {
                    // middle
                    m_pageStart = m_pageIndex - pageMove;
                }
                m_pageEnd = m_pageStart + m_pageShow - 1;
            }
        }

        public void Show(Action<SPageNode> act) {

            // first...
            if(m_pageStart > m_first) {
                string show = m_first.ToString();
                if(m_pageStart > m_first + 1) {
                    show += m_more;
                }
                act(new SPageNode(m_first, show));
            }

            // pages
            for(int i = m_pageStart; i <= m_pageEnd; ++i) {
                bool active = m_pageIndex == i;
                act(new SPageNode(i, i.ToString(), active));
            }

            // ...last
            if (m_pageEnd < m_pageTotal) {
                string show = m_pageTotal.ToString();
                if (m_pageEnd < m_pageTotal - 1) {
                    show = m_more + show;
                }
                act(new SPageNode(m_pageTotal, show));
            }

            // next
            if (m_pageIndex < m_pageEnd) {
                act(new SPageNode(m_pageIndex + 1, m_next));
            }

        }
    }
}