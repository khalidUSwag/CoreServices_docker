﻿using Catalog.Api.Data;
using Catalog.Api.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly ICatalogContext _context;
        public ProductRepository(ICatalogContext catatogContext)
        {
            _context = catatogContext;
        }

        public async Task CreateProduct(Product product)
        {
             await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            DeleteResult deleteResult =await _context.Products.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);
            return await _context.Products.Find(filter).ToListAsync();

        }        
        
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.Find(p => true).ToListAsync();
        }
        public async Task<Product> GetProductById(string id)
        {
            return await _context.Products.Find(e => e.Id == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            //return await _context.Products.Find(p => p.Name == name).ToListAsync();

            //use filter option
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await _context.Products.Find(filter).ToListAsync();

        }
        public Task<Product> GetProductCategory(string categoty)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updatedProduct=await _context.Products.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
        }        
    }
}
