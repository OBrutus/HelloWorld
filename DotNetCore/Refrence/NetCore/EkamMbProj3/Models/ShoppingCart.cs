﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EkamArtMBProj.Models
{
    public class ShoppingCart
    {
        private readonly PaintingDbContext _paintingDbContext;

        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ShoppingCart(PaintingDbContext paintingDbContext)
        {
            _paintingDbContext = paintingDbContext;
        }

        //GetCart/AddtoCart/RemoveFromcart/GetShoppingitems/clear cart/gettotal

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<PaintingDbContext>();

            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            return new ShoppingCart(context) { ShoppingCartId = cartId };
            
        }

        public void AddToCart(Painting painting,int amount)
        {
            var shoppingCartItem =
                _paintingDbContext.ShoppingCartItems.SingleOrDefault(
                    s => s.Painting.PaintingId == painting.PaintingId && s.ShoppingCartId == ShoppingCartId);
            if(shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Painting = painting,
                    Amount = 1
                };
                _paintingDbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            _paintingDbContext.SaveChanges();
        }


    }
}
