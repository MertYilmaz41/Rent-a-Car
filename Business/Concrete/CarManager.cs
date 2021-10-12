using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class CarManager : ICarService
    {
        ICarDal _carDal;
        public CarManager(ICarDal carDal)
        {
            _carDal = carDal;
        }


        [ValidationAspect(typeof(CarValidator))]
        //[SecuredOperation("car.add,admin")]
        [CacheRemoveAspect("ICarService.Get")]
        [TransactionAspect]
        
        public IResult Add(Car car)
        {
            _carDal.Add(car);

            return new SuccessResult(Messages.CarAdded);
        }

        [CacheRemoveAspect("ICarService.Get")]
        public IResult Delete(Car car)
        {
            _carDal.Delete(car);

            return new SuccessResult(Messages.CarDeleted);
        }

        [CacheRemoveAspect("ICarService.Get")]
        public IResult Update(Car car)
        {
            _carDal.Update(car);
            return new SuccessResult(Messages.CarUpdated);
        }

        //[SecuredOperation("car.getall")]
        [CacheAspect]
        
        public IDataResult<List<Car>> GetAll()
        {
             return new SuccessDataResult<List<Car>>(_carDal.GetAll(),Messages.CarsListed);
        }

        [CacheAspect]
        public IDataResult<Car>GetById(int carId)
        {
            return new SuccessDataResult<Car> (_carDal.Get(c => c.CarId == carId));
        }

        [CacheAspect]
        public IDataResult<List<Car>> GetCarsByBrandId(int brandId)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(c => c.BrandId == brandId));
        }

        [CacheAspect]
        public IDataResult<List<Car>> GetCarsByColorId(int id)
        {
            return new SuccessDataResult<List<Car>> (_carDal.GetAll(c => c.ColorId == id));
        }

        [CacheAspect]
        public IDataResult<List<Car>> GetCarsByDailyPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(c => c.DailyPrice >= min && c.DailyPrice <= max));
        }

        [CacheAspect]
        public IDataResult<List<CarDetailDto>> GetCarDetails()
        {
            var result = _carDal.GetCarDetails();
            return new SuccessDataResult<List<CarDetailDto>>(result, Messages.Success + "\n" + Messages.CarsListed);
        }

        [CacheAspect]
        public IDataResult<List<CarDetailDto>> GetCarDetailsByBrandId(int brandId)
        {
            var result = _carDal.GetCarDetails(c => c.BrandId == brandId);
            return new SuccessDataResult<List<CarDetailDto>>(result, Messages.Success);
        }

        [CacheAspect]
        public IDataResult<List<CarDetailDto>> GetCarDetailsByColorId(int colorId)
        {
            var result = _carDal.GetCarDetails(c => c.ColorId == colorId);
            return new SuccessDataResult<List<CarDetailDto>>(result, Messages.Success);
        }

        public IDataResult<CarDetailDto> GetCarDetailById(int id)
        {
            var result = _carDal.GetCarDetail(c => c.CarId == id);
            return new SuccessDataResult<CarDetailDto>(result, Messages.Success);
        }

        
    }
}
