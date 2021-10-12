using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    //NuGet
    public class EfCarDal : EfEntityRepositoryBase<Car, CarsContext>, ICarDal
    {
        public CarDetailDto GetCarDetail(Expression<Func<Car, bool>> filter)
        {
            using (CarsContext context = new CarsContext())
            {
                var result = from car in context.Cars.Where(filter)
                             join brand in context.Brands
                             on car.BrandId equals brand.BrandId
                             join color in context.Colors
                             on car.ColorId equals color.ColorId
                             select new CarDetailDto { CarId = car.CarId, CarName = car.Description, BrandName = brand.BrandName, ColorName = color.ColorName, DailyPrice = car.DailyPrice, ModelYear = car.ModelYear};
                return result.SingleOrDefault();
            }
        }


        public List<CarDetailDto> GetCarDetails(Expression<Func<Car, bool>> filter = null)
        {
            using (CarsContext context = new CarsContext())
            {
                var result = from car in filter == null ? context.Cars : context.Cars.Where(filter)
                             join brand in context.Brands
                             on car.BrandId equals brand.BrandId
                             join color in context.Colors
                             on car.ColorId equals color.ColorId
                             select new CarDetailDto { CarId = car.CarId, CarName = car.Description, BrandName = brand.BrandName, ColorName = color.ColorName, DailyPrice = car.DailyPrice, ModelYear = car.ModelYear };
                return result.ToList();
            }
        }
    }
}
