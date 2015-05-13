﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Mediachase.Commerce;
using OxxCommerceStarterKit.Core.Extensions;
using OxxCommerceStarterKit.Web.Models.Blocks.Contracts;
using OxxCommerceStarterKit.Web.Models.FindModels;
using OxxCommerceStarterKit.Web.Models.ViewModels;

namespace OxxCommerceStarterKit.Web.Models.Catalog
{
    [CatalogContentType(DisplayName = "Generic size variation",
      GUID = "6C00EADF-9246-42FF-8833-CB5FEA79B1C7",
      MetaClassName = "GenericSizeVariationContent")]
    public class GenericSizeVariationContent : VariationContent, IIndexableContent, IProductListViewModelInitializer, IResourceable
    {
        [Display(Name = "Show in product list",
         Order = 10)]
        [DefaultValue(true)]
        public virtual bool ShowInList { get; set; }

        // Same for all languages
        [Display(Name = "Facet Brand",
            Order = 15)]
        public virtual string Facet_Brand { get; set; }

        [Display(Name = "Size", Order = 20)]
        public virtual string Size { get; set; }

        [Display(Name = "Color", Order = 30)]
        [CultureSpecific]
        public virtual string Color { get; set; }

        [Display(
        GroupName = SystemTabNames.Content,
        Order = 80,
        Name = "Description")]
        [CultureSpecific]
        public virtual XhtmlString Description { get; set; }

        // Multi lang
        [Display(Name = "Overview", Order = 100)]
        [CultureSpecific]
        public virtual XhtmlString Overview { get; set; }

        // Multi lang
        [Display(Name = "Details", Order = 120)]
        [CultureSpecific]
        public virtual XhtmlString Details { get; set; }

        [Display(
           GroupName = SystemTabNames.Content,
           Order = 300,
           Name = "Average Rating")]
        [Editable(false)]
        public virtual double AverageRating { get; set; }

        public FindProduct GetFindProduct(Mediachase.Commerce.IMarket market)
        {
            var language = (Language == null ? string.Empty : Language.Name);
            var findProduct = new FindProduct(this,language);

            findProduct.Description = Description;
            findProduct.Overview = Overview;
            findProduct.ShowInList = ShowInList;
            EPiServer.Commerce.SpecializedProperties.Price defaultPrice = this.GetDefaultPrice();
            findProduct.DefaultPrice = this.GetDisplayPrice(market);
            findProduct.DefaultPriceAmount = this.GetDefaultPriceAmount(market);
            findProduct.DiscountedPrice = this.GetDiscountDisplayPrice(defaultPrice, market);
            findProduct.CustomerClubPrice = this.GetCustomerClubDisplayPrice(market);
            findProduct.Brand = this.Facet_Brand;
            return findProduct;
        }

        public bool ShouldIndex()
        {
            return !(StopPublish != null && StopPublish < DateTime.Now);
        }

        public ProductListViewModel Populate(Mediachase.Commerce.IMarket currentMarket)
        {
            UrlResolver urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();

            ProductListViewModel productListViewModel = new ProductListViewModel
            {
                Code = this.Code,
                ContentLink = this.ContentLink,
                DisplayName = this.DisplayName,
                Description = Description,
                ProductUrl = urlResolver.GetUrl(ContentLink),
                ImageUrl = this.GetDefaultImage(),
                PriceString = this.GetDisplayPrice(currentMarket),
                ContentType = this.GetType().Name
            };

            productListViewModel.PriceAmount = this.GetDefaultPriceAmount(currentMarket);
            return productListViewModel;
        }


        public virtual string ContentAssetIdInternal { get; set; }
        public Guid ContentAssetsID
        {
            get
            {
                Guid assetId;
                if (Guid.TryParse(ContentAssetIdInternal, out assetId))
                    return assetId;
                return Guid.Empty;
            }
            set
            {
                ContentAssetIdInternal = value.ToString();
                this.ThrowIfReadOnly();
                IsModified = true;
            }
        }
    }
}