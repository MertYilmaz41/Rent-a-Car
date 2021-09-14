using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Helpers;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Business.Concrete
{
    
    public class CarImageManager : ICarImageService
    {
        ICarImageDal _carImageDal;
        public CarImageManager(ICarImageDal carImageDal)
        {
            _carImageDal = carImageDal;
        }

        [ValidationAspect(typeof(CarImageValidator))]
        [TransactionAspect]
        public IResult Add(CarImage carImage, IFormFile file)
        {
            var result = BusinessRules.Run(CheckIfCarImageLimitExceeded(carImage.CarId));
            if (result != null) return result;

            carImage.ImagePath = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            carImage.Date = DateTime.Now;

            _carImageDal.Add(carImage);

            var imageUploadResult = FileHelper.Upload(carImage.ImagePath, file);
            if (!imageUploadResult.Success) throw new Exception(imageUploadResult.Message);

            return new SuccessResult(Messages.CarImageAdded);
        }

        [TransactionAspect]
        public IResult Update(int id, IFormFile file)
        {
            var carImage = _carImageDal.Get(ci => ci.CarId == id);

            _carImageDal.Update(carImage);

            var imageUpdateResult = FileHelper.Update(carImage.ImagePath, file);
            if (!imageUpdateResult.Success) throw new Exception(imageUpdateResult.Message);

            return new SuccessResult(Messages.CarImageUpdated);
        }

        [TransactionAspect]
        public IResult Delete(int id)
        {
            var carImage = _carImageDal.Get(ci => ci.CarId == id);
            _carImageDal.Delete(carImage);

            var imageDeleteResult = FileHelper.Delete(carImage.ImagePath);
            if (!imageDeleteResult.Success) throw new Exception(imageDeleteResult.Message);

            return new SuccessResult(Messages.CarImageDeleted);
        }


        public IDataResult<List<CarImage>> GetAll()
        {
            return new SuccessDataResult<List<CarImage>>(_carImageDal.GetAll(), Messages.CarImagesListed);
        }

        public IDataResult<CarImage> GetById(int id)
        {
            return new SuccessDataResult<CarImage>(_carImageDal.Get(cı => cı.CarImageId == id), Messages.Success);
        }

        private IResult CheckIfCarImageLimitExceeded(int carId)
        {
            var result = _carImageDal.GetAll(c => c.CarId == carId).Count;
            if (result >= 5)
            {
                return new ErrorResult(Messages.CarImageLimitExceeded);
            }

            return new SuccessResult();
        }

        public IDataResult<List<CarImage>> GetByCarId(int carId)
        {
            return new SuccessDataResult<List<CarImage>>(_carImageDal.GetAll(c => c.CarId == carId));
        }
       
    }
}
