using System;
using UIKit;
using CoreGraphics;

namespace IQAudioRecorderController {
    
    
    public class SCSiriWaveformView : UIView {
        
        #region Fields
		private nuint _NumberOfWaves;
        
        private UIColor _WaveColor;
        
        private nfloat _PrimaryWaveLineWidth;
        
        private nfloat _SecondaryWaveLineWidth;
        
        private nfloat _IdleAmplitude;
        
        private nfloat _Frequency;
        
        private nfloat _Amplitude;
        
        private nfloat _Density;
        
        private nfloat _PhaseShift;
        
        private nfloat _Phase;
        #endregion
        
        #region Constructors
		public SCSiriWaveformView() 
			: base()
		{
			Setup();
        }
        
		public SCSiriWaveformView(CGRect frame) 
			: base(frame)
		{
			Setup();
        }
        #endregion
        
        #region Properties


		public nuint NumberOfWaves {
            get {
                return this._NumberOfWaves;
            }
            set {
                this._NumberOfWaves = value;
            }
        }
        
        public UIColor WaveColor {
            get {
                return this._WaveColor;
            }
            set {
                this._WaveColor = value;
            }
        }
        
        public nfloat PrimaryWaveLineWidth {
            get {
                return this._PrimaryWaveLineWidth;
            }
            set {
                this._PrimaryWaveLineWidth = value;
            }
        }
        
        public nfloat SecondaryWaveLineWidth {
            get {
                return this._SecondaryWaveLineWidth;
            }
            set {
                this._SecondaryWaveLineWidth = value;
            }
        }
        
        public nfloat IdleAmplitude {
            get {
                return this._IdleAmplitude;
            }
            set {
                this._IdleAmplitude = value;
            }
        }
        
        public nfloat Frequency {
            get {
                return this._Frequency;
            }
            set {
                this._Frequency = value;
            }
        }
        
        public nfloat Amplitude {
            get {
                return this._Amplitude;
            }
        }
        
        public nfloat Density {
            get {
                return this._Density;
            }
            set {
                this._Density = value;
            }
        }
        
        public nfloat PhaseShift {
            get {
                return this._PhaseShift;
            }
            set {
                this._PhaseShift = value;
            }
        }
        
        private nfloat Phase {
            get {
                return this._Phase;
            }
            set {
                this._Phase = value;
            }
        }
        #endregion
        
        #region Methods
        public override void AwakeFromNib() {

			Setup();

        }
        
        private void Setup() {
             
         
             this.Frequency = 1.5f;
             
			this._Amplitude = 1.0f;
             this.IdleAmplitude = 0.01f;
             
             this.NumberOfWaves = 5;
             this.PhaseShift = -0.15f;
             this.Density = 5.0f;
             
			this.WaveColor = UIColor.White;
             this.PrimaryWaveLineWidth = 3.0f;
             this.SecondaryWaveLineWidth = 1.0f;
             
        }
        
        private void UpdateWithLevel(nfloat level)
		{
			
            this.Phase += this.PhaseShift;
			this._Amplitude = (nfloat)Math.Max( level, this.IdleAmplitude);
                 
			SetNeedsDisplay();

        }
        
		public override void Draw(CGRect rect)
		{
			base.Draw(rect);

		     var context = UIGraphics.GetCurrentContext();

			context.ClearRect(this.Bounds);

			BackgroundColor.SetFill();
			context.FillRect(rect);



		 	for(nuint i=0; i < this.NumberOfWaves; i++) {
		 		
		         var context2 = UIGraphics.GetCurrentContext();
		         
				context2.SetLineWidth(i==0 ? this.PrimaryWaveLineWidth : this.SecondaryWaveLineWidth);
		 		
				var halfHeight = this.Bounds.Height / 2.0f;
				var width = this.Bounds.Width;
				var mid = width / 2.0f;
		 		
		 		var maxAmplitude = halfHeight - 4.0f; // 4 corresponds to twice the stroke width
		 		
		 		// Progress is a value between 1.0 and -0.5, determined by the current wave idx, which is used to alter the wave's amplitude.
		 		var progress = 1.0f - i / this.NumberOfWaves;
		 		var normedAmplitude = (1.5f * progress - 0.5f) * this.Amplitude;
		 		
		         var multiplier = Math.Min(1.0, (progress / 3.0f * 2.0f) + (1.0f / 3.0f));

				this.WaveColor.ColorWithAlpha((nfloat)multiplier * this.WaveColor.CGColor.Alpha).SetFill();

		        // [[this.waveColor colorWithAlphaComponent:multiplier * CGColorGetAlpha(this.waveColor.CGColor)] set];
		 		
		 		for(Double x = 0; x<width + this.Density; x += this.Density) {
		 			
		 			// We use a parable to scale the sinus wave, that has its peak in the middle of the view.
					var scaling = -Math.Pow(1 / mid * (x - mid), 2) + 1;
		 						
					var y = scaling * maxAmplitude * normedAmplitude * Math.Sin(2 * Math.PI * (x / width) * this.Frequency + this.Phase) + halfHeight;
		 			
		 			if (x==0) 
					{
						context2.MoveTo( (nfloat)x, (nfloat)y);
		             }
		 			else {
						context2.AddLineToPoint((nfloat)x, (nfloat)y);
		             }
		 		}
			}


			context.StrokePath();
        }
        #endregion
    }
}
