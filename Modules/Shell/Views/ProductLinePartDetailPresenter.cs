using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web.UI.WebControls;
using System.Web;
using System.Linq;
using System.Transactions;

namespace VCTWebApp.Shell.Views
{
    public class ProductLinePartDetailPresenter : Presenter<IProductLinePartDetail>
    {
        #region Instance Variables

        private Helper helper = new Helper();
        ProductLinePartDetailRepository productLinePartDetailRepository = null;
        private TreeNodeCollection locationsTreeNodes;

        #endregion

        #region Constructors

        public ProductLinePartDetailPresenter()
            : this(new ProductLinePartDetailRepository())
        {
        }

        public ProductLinePartDetailPresenter(VCTWeb.Core.Domain.ProductLinePartDetailRepository _productLinePartDetailRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ProductLinePartDetailPresenter", "Constructor is invoked.");
            this.productLinePartDetailRepository = _productLinePartDetailRepository;
        }

        #endregion

        #region Public Overrides

        public override void OnViewLoaded()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ProductLinePartDetailPresenter", "OnViewLoaded is invoked.");
        }

        public override void OnViewInitialized()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ProductLinePartDetailPresenter", "OnViewInitialized() is invoked.");
            BindTreeNodeCollection();
            View.ListProductLinePartDetail = new List<ProductLinePartDetail>();
        }

        #endregion

        #region Public Methods

        public void FetchFilteredDataForGrid()
        {
            List<VCTWeb.Core.Domain.ProductLinePartDetail> listFilteredProductLinePartDetail = new List<ProductLinePartDetail>();
            List<VCTWeb.Core.Domain.ProductLinePartDetail> listProductLinePartDetail = this.productLinePartDetailRepository.FetchAllProductLinePartDetail();

            switch (View.SelectedNodeLevel)
            {
                case 0:
                    listFilteredProductLinePartDetail = listProductLinePartDetail.FindAll(i => i.ProductLineName == View.SelectedProductLine);
                    break;

                case 1:
                    listFilteredProductLinePartDetail = listProductLinePartDetail.FindAll(i => i.ProductLineName == View.SelectedProductLine && i.Category == View.SelectedCategory);
                    break;

                case 2:
                    listFilteredProductLinePartDetail = listProductLinePartDetail.FindAll(i => i.ProductLineName == View.SelectedProductLine && i.Category == View.SelectedCategory && i.SubCategory1 == View.SelectedSubCategory1);
                    break;

                case 3:
                    listFilteredProductLinePartDetail = listProductLinePartDetail.FindAll(i => i.ProductLineName == View.SelectedProductLine && i.Category == View.SelectedCategory && i.SubCategory1 == View.SelectedSubCategory1 && i.SubCategory2 == View.SelectedSubCategory2);
                    break;

                case 4:
                    listFilteredProductLinePartDetail = listProductLinePartDetail.FindAll(i => i.ProductLineName == View.SelectedProductLine && i.Category == View.SelectedCategory && i.SubCategory1 == View.SelectedSubCategory1 && i.SubCategory2 == View.SelectedSubCategory2 && i.SubCategory3 == View.SelectedSubCategory3);
                    break;
            }
            View.ListProductLinePartDetail = listFilteredProductLinePartDetail;
        }

        public bool UpdateParLevelQuantityForRefNum(string RefNum, int PARLevelQty)
        {
            bool isUpdated = false;
            isUpdated = productLinePartDetailRepository.UpdateParLevelQuantityForRefNum(RefNum, PARLevelQty);
            return isUpdated;
        }

        #endregion

        #region Private Methods

        private void BindTreeNodeCollection()
        {
            locationsTreeNodes = new TreeNodeCollection();
            List<VCTWeb.Core.Domain.ProductLinePartDetail> listProductLinePartDetail = this.productLinePartDetailRepository.FetchAllProductLinePartDetail();

            var listLevel1 = listProductLinePartDetail.Select(s => new { ProductLine = s.ProductLineName, ProductLineDesc = s.ProductLineDesc }).Distinct();

            foreach (var level1 in listLevel1)
            {
                TreeNode Level1Node = new TreeNode(level1.ProductLine + ' ' + level1.ProductLineDesc, level1.ProductLine);
                locationsTreeNodes.Add(Level1Node);

                var list2 = listProductLinePartDetail.FindAll(p => p.ProductLineName == level1.ProductLine && p.ProductLineDesc == level1.ProductLineDesc);
                var listLevel2 = list2.Select(s => new { ProductLine = s.ProductLineName, ProductLineDesc = s.ProductLineDesc, Category = s.Category }).Distinct();

                foreach (var level2 in listLevel2)
                {
                    if (!string.IsNullOrEmpty(level2.Category))
                    {
                        TreeNode Level2Node = new TreeNode(level2.Category, level2.Category);
                        Level1Node.ChildNodes.Add(Level2Node);

                        var list3 = listProductLinePartDetail.FindAll(p => p.ProductLineName == level2.ProductLine && p.ProductLineDesc == level2.ProductLineDesc && p.Category == level2.Category);
                        var listLevel3 = list3.Select(s => new { ProductLine = s.ProductLineName, ProductLineDesc = s.ProductLineDesc, Category = s.Category, SubCategory1 = s.SubCategory1 }).Distinct();
                        foreach (var level3 in listLevel3)
                        {
                            if (!string.IsNullOrEmpty(level3.SubCategory1))
                            {
                                TreeNode Level3Node = new TreeNode(level3.SubCategory1, level3.SubCategory1);
                                Level2Node.ChildNodes.Add(Level3Node);

                                var list4 = listProductLinePartDetail.FindAll(p => p.ProductLineName == level3.ProductLine && p.ProductLineDesc == level3.ProductLineDesc && p.Category == level3.Category && p.SubCategory1 == level3.SubCategory1);
                                var listLevel4 = list4.Select(s => new { ProductLine = s.ProductLineName, ProductLineDesc = s.ProductLineDesc, Category = s.Category, SubCategory1 = s.SubCategory1, SubCategory2 = s.SubCategory2 }).Distinct();

                                foreach (var level4 in listLevel4)
                                {
                                    if (!string.IsNullOrEmpty(level4.SubCategory2))
                                    {
                                        TreeNode Level4Node = new TreeNode(level4.SubCategory2, level4.SubCategory2);
                                        Level3Node.ChildNodes.Add(Level4Node);

                                        var list5 = listProductLinePartDetail.FindAll(p => p.ProductLineName == level4.ProductLine && p.ProductLineDesc == level4.ProductLineDesc && p.Category == level4.Category && p.SubCategory1 == level4.SubCategory1 && p.SubCategory2 == level4.SubCategory2);
                                        var listLevel5 = list5.Select(s => new { ProductLine = s.ProductLineName, ProductLineDesc = s.ProductLineDesc, Category = s.Category, SubCategory1 = s.SubCategory1, SubCategory2 = s.SubCategory2, SubCategory3 = s.SubCategory3 }).Distinct();

                                        foreach (var level5 in listLevel5)
                                        {
                                            if (!string.IsNullOrEmpty(level5.SubCategory3))
                                            {
                                                TreeNode Level5Node = new TreeNode(level5.SubCategory3, level5.SubCategory3);
                                                Level4Node.ChildNodes.Add(Level5Node);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            View.ProductLinePartDetailNodeList = locationsTreeNodes;
        }

        #endregion


    }
}
