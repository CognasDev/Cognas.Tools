namespace Cognas.ApiTools.Shared.Patterns
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TIType"></typeparam>
    public abstract class FactoryBase<TIType> : IFactory<TIType>
    {
        #region Constructor / Finaliser Declarations

        /// <summary>
        /// Default constructor for <see cref="FactoryBase{TIType}"/>
        /// </summary>
        protected FactoryBase()
        {

        }

        #endregion

        #region Public Method Declarations

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TCreate"></typeparam>
        /// <returns></returns>
        public abstract TIType Create<TCreate>() where TCreate : TIType;

        #endregion
    }
}